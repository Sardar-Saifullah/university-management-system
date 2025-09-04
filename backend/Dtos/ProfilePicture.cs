// Dtos/ProfilePicture/ProfilePictureUploadDto.cs
using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class ProfilePictureUploadDto
    {
        [Required]
        public int TargetUserId { get; set; }

        [Required]
        public IFormFile ImageFile { get; set; } = null!;
    }

    public class ProfilePictureUpdateDto
    {
        [Required]
        public int PictureId { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }

    public class ProfilePictureResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string MimeType { get; set; } = string.Empty;
        public string StorageType { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime UploadedAt { get; set; }
        public string UploadedByName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public DateTime ModifiedAt { get; set; }
        public string ModifiedByName { get; set; } = string.Empty;
    }

    public class ImageUploadResult
    {
        public string FilePath { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSize { get; set; }
    }

  
}