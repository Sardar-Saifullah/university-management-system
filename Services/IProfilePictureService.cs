using backend.Dtos;
using System.Threading.Tasks;

namespace backend.Services
{
    public interface IProfilePictureService
    {
        Task<(bool Success, int PictureId, string Message)> CreateProfilePictureAsync(int adminId, ProfilePictureUploadDto dto);
        Task<(bool Success, ProfilePictureResponseDto Data, string Message)> GetProfilePictureAsync(int requestingUserId, int targetUserId);
        Task<(bool Success, ProfilePictureResponseDto Data, string Message)> GetMyProfilePictureAsync(int userId);
        Task<(bool Success, string Message)> DeleteProfilePictureAsync(int adminId, int pictureId);
    }
}