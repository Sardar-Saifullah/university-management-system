using backend.Dtos;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace backend.Utilities
{
    public interface IImageProcessor
    {
        Task<ImageUploadResult> ProcessAndSaveProfileImage(IFormFile imageFile, int userId, string profileType);
        Task<bool> DeleteProfileImage(string imagePath);
    }

    public class ImageProcessor : IImageProcessor
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _uploadsFolder;

        private const int MaxFileSize = 5 * 1024 * 1024; // 5MB
        private const int MaxDimension = 800;
        private static readonly string[] AllowedContentTypes = { "image/jpeg", "image/png" };

        public ImageProcessor(IWebHostEnvironment environment)
        {
            _environment = environment;
            // Ensure uploads directory exists
            _uploadsFolder = Path.Combine(_environment.WebRootPath ??
                            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
                            "uploads", "profile-pictures");

            Directory.CreateDirectory(_uploadsFolder);
        }

        public async Task<ImageUploadResult> ProcessAndSaveProfileImage(IFormFile imageFile, int userId, string profileType)
        {
            ValidateImageFile(imageFile);

            using var imageStream = new MemoryStream();
            await imageFile.CopyToAsync(imageStream);
            imageStream.Position = 0;

            // Process image
            using var image = await Image.LoadAsync(imageStream);
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(MaxDimension, MaxDimension),
                Mode = ResizeMode.Max,
                Sampler = KnownResamplers.Lanczos3
            }));

            // Determine file extension and encoder
            string fileExtension = imageFile.ContentType == "image/png" ? ".png" : ".jpg";
            IImageEncoder encoder = fileExtension.Equals(".png", StringComparison.OrdinalIgnoreCase)
                ? new PngEncoder()
                : new JpegEncoder { Quality = 75 };

            // Save to disk
            var uploadsFolder = Path.Combine(_environment.WebRootPath ??
                               Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
                               "uploads", "profile-pictures", profileType.ToLower());

            Directory.CreateDirectory(uploadsFolder);
            var fileName = $"{userId}_{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await image.SaveAsync(fileStream, encoder);

            return new ImageUploadResult
            {
                FilePath = $"/uploads/profile-pictures/{profileType.ToLower()}/{fileName}",
                ContentType = fileExtension == ".png" ? "image/png" : "image/jpeg",
                FileSize = fileStream.Length
            };
        }

        public async Task<bool> DeleteProfileImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return false;

            var fullPath = Path.Combine(_environment.WebRootPath, imagePath.TrimStart('/'));

            if (!File.Exists(fullPath))
                return false;

            try
            {
                File.Delete(fullPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void ValidateImageFile(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("No image file provided");

            if (imageFile.Length > MaxFileSize)
                throw new ArgumentException($"Image exceeds maximum size of {MaxFileSize / 1024 / 1024}MB");

            if (!AllowedContentTypes.Contains(imageFile.ContentType.ToLower()))
                throw new ArgumentException("Only JPEG and PNG images are allowed");
        }
    }

   
}