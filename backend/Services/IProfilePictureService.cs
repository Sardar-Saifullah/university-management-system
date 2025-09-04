using backend.Dtos;

namespace backend.Services
{
    public interface IProfilePictureService
    {
        Task<ProfilePictureResponseDto> UploadProfilePictureAsync(int adminId, ProfilePictureUploadDto uploadDto);
        Task<ProfilePictureResponseDto> UpdateProfilePictureAsync(int adminId, ProfilePictureUpdateDto updateDto);
        Task<bool> DeleteProfilePictureAsync(int adminId, int pictureId);
        Task<ProfilePictureResponseDto> GetActiveProfilePictureAsync(int userId, int targetUserId);
        Task<List<ProfilePictureResponseDto>> GetUserProfilePicturesAsync(int userId, int targetUserId);
        Task<List<ProfilePictureResponseDto>> GetAllProfilePicturesAsync(int adminId, string? userType, bool? isActive);
    }
}
