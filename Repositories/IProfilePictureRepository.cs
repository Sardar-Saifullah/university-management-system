using backend.Models;
namespace backend.Repositories
{
    public interface IProfilePictureRepository
    {
        Task<int> CreateProfilePictureAsync(int adminId, int userId, string fileName,
            string filePath, long fileSize, string mimeType, string storageType);
        Task<bool> UpdateProfilePictureAsync(int adminId, int pictureId, bool isActive);
        Task<(bool success, string filePath)> DeleteProfilePictureAsync(int adminId, int pictureId);
        Task<ProfilePicture?> GetActiveProfilePictureAsync(int userId);
        Task<List<ProfilePictureResponse>> GetProfilePicturesByUserAsync(int userId);
        Task<List<ProfilePictureResponse>> GetAllProfilePicturesAsync(string? userType, bool? isActive);
    }
}
