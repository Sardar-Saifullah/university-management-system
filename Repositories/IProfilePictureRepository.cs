using backend.Models;
using System.Threading.Tasks;

namespace backend.Repositories
{
    public interface IProfilePictureRepository
    {
        Task<(bool Success, int PictureId, string Message)> CreateProfilePictureAsync(int adminId, ProfilePicture picture);
        Task<(bool Success, ProfilePictureResponse Data, string Message)> GetProfilePictureAsync(int requestingUserId, int targetUserId);
        Task<(bool Success, ProfilePictureResponse Data, string Message)> GetMyProfilePictureAsync(int userId);
        Task<(bool Success, string FilePath, string Message)> DeleteProfilePictureAsync(int adminId, int pictureId);
    }
}