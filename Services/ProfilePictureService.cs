using backend.Dtos;
using backend.Models;
using backend.Repositories;
using backend.Utilities;
using System.Threading.Tasks;

namespace backend.Services
{
    public class ProfilePictureService : IProfilePictureService
    {
        private readonly IProfilePictureRepository _profilePictureRepository;
        private readonly IImageProcessor _imageProcessor;

        public ProfilePictureService(IProfilePictureRepository profilePictureRepository, IImageProcessor imageProcessor)
        {
            _profilePictureRepository = profilePictureRepository;
            _imageProcessor = imageProcessor;
        }

        public async Task<(bool Success, int PictureId, string Message)> CreateProfilePictureAsync(int adminId, ProfilePictureUploadDto dto)
        {
            var uploadResult = await _imageProcessor.ProcessAndSaveProfileImage(dto.ImageFile, dto.TargetUserId, "user");

            var picture = new ProfilePicture
            {
                UserId = dto.TargetUserId,
                FileName = Path.GetFileName(uploadResult.FilePath),
                FilePath = uploadResult.FilePath,
                FileSize = uploadResult.FileSize,
                MimeType = uploadResult.ContentType,
                StorageType = "local",
                UploadedBy = adminId,
                CreatedBy = adminId,
                ModifiedBy = adminId
            };

            return await _profilePictureRepository.CreateProfilePictureAsync(adminId, picture);
        }

        public async Task<(bool Success, ProfilePictureResponseDto Data, string Message)> GetProfilePictureAsync(int requestingUserId, int targetUserId)
        {
            var (success, data, message) = await _profilePictureRepository.GetProfilePictureAsync(requestingUserId, targetUserId);
            if (!success)
                return (false, null, message);

            var response = new ProfilePictureResponseDto
            {
                Id = data.Id,
                UserId = data.UserId,
                UserName = data.UserName,
                UserType = data.UserType,
                FileName = data.FileName,
                FilePath = data.FilePath,
                FileSize = data.FileSize,
                MimeType = data.MimeType,
                StorageType = data.StorageType,
                IsActive = data.IsActive,
                IsCurrentActive = data.IsCurrentActive,
                UploadedAt = data.UploadedAt,
                UploadedByName = data.UploadedByName,
                CreatedAt = data.CreatedAt,
                CreatedByName = data.CreatedByName,
                ModifiedAt = data.ModifiedAt,
                ModifiedByName = data.ModifiedByName
            };

            return (true, response, "Profile picture retrieved successfully");
        }

        public async Task<(bool Success, ProfilePictureResponseDto Data, string Message)> GetMyProfilePictureAsync(int userId)
        {
            var (success, data, message) = await _profilePictureRepository.GetMyProfilePictureAsync(userId);
            if (!success)
                return (false, null, message);

            var response = new ProfilePictureResponseDto
            {
                Id = data.Id,
                UserId = data.UserId,
                UserName = data.UserName,
                UserType = data.UserType,
                FileName = data.FileName,
                FilePath = data.FilePath,
                FileSize = data.FileSize,
                MimeType = data.MimeType,
                StorageType = data.StorageType,
                IsActive = data.IsActive,
                IsCurrentActive = data.IsCurrentActive,
                UploadedAt = data.UploadedAt
            };

            return (true, response, "Profile picture retrieved successfully");
        }

        public async Task<(bool Success, string Message)> DeleteProfilePictureAsync(int adminId, int pictureId)
        {
            try
            {
                var (success, filePath, message) = await _profilePictureRepository.DeleteProfilePictureAsync(adminId, pictureId);

                if (success && !string.IsNullOrEmpty(filePath))
                {
                    await _imageProcessor.DeleteProfileImage(filePath);
                }

                return (success, message);
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting profile picture: {ex.Message}");
            }
        }
    }
}