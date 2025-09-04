// Repositories/Implementations/ProfilePictureRepository.cs
using backend.Data;
using backend.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace backend.Repositories
{
    public class ProfilePictureRepository : IProfilePictureRepository
    {
        private readonly IDatabaseContext _context;

        public ProfilePictureRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<int> CreateProfilePictureAsync(int adminId, int userId, string fileName, 
            string filePath, long fileSize, string mimeType, string storageType)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_admin_id", adminId),
                new MySqlParameter("p_user_id", userId),
                new MySqlParameter("p_file_name", fileName),
                new MySqlParameter("p_file_path", filePath),
                new MySqlParameter("p_file_size", fileSize),
                new MySqlParameter("p_mime_type", mimeType),
                new MySqlParameter("p_storage_type", storageType),
                new MySqlParameter("p_result", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_message", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_picture_id", MySqlDbType.Int32) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_create_profile_picture", parameters);

            var result = Convert.ToInt32(parameters.First(p => p.ParameterName == "p_result").Value);
            var message = parameters.First(p => p.ParameterName == "p_message").Value?.ToString();
            var pictureId = Convert.ToInt32(parameters.First(p => p.ParameterName == "p_picture_id").Value);

            if (result == 0)
                throw new Exception(message ?? "Failed to create profile picture");

            return pictureId;
        }

        public async Task<bool> UpdateProfilePictureAsync(int adminId, int pictureId, bool isActive)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_admin_id", adminId),
                new MySqlParameter("p_picture_id", pictureId),
                new MySqlParameter("p_is_active", isActive),
                new MySqlParameter("p_result", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_message", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_update_profile_picture", parameters);

            var result = Convert.ToInt32(parameters.First(p => p.ParameterName == "p_result").Value);
            var message = parameters.First(p => p.ParameterName == "p_message").Value?.ToString();

            if (result == 0)
                throw new Exception(message ?? "Failed to update profile picture");

            return result == 1;
        }

        public async Task<(bool success, string filePath)> DeleteProfilePictureAsync(int adminId, int pictureId)
        {
            var outputParam = new MySqlParameter("file_path", MySqlDbType.VarChar, 500)
            {
                Direction = ParameterDirection.Output
            };

            var parameters = new[]
            {
        new MySqlParameter("p_admin_id", adminId),
        new MySqlParameter("p_picture_id", pictureId),
        new MySqlParameter("p_result", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
        new MySqlParameter("p_message", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Output },
        outputParam
    };

            await _context.ExecuteNonQueryAsync("sp_delete_profile_picture", parameters);

            var result = Convert.ToInt32(parameters.First(p => p.ParameterName == "p_result").Value);
            var message = parameters.First(p => p.ParameterName == "p_message").Value?.ToString();
            var filePath = outputParam.Value?.ToString() ?? string.Empty;

            if (result == 0)
                throw new Exception(message ?? "Failed to delete profile picture");

            return (result == 1, filePath);
        }

        public async Task<ProfilePicture?> GetActiveProfilePictureAsync(int userId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_requesting_user_id", userId),
                new MySqlParameter("p_target_user_id", userId),
                new MySqlParameter("p_result", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_message", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Output }
            };

            var result = await _context.ExecuteQueryAsync("sp_get_profile_picture", parameters);

            var success = Convert.ToInt32(parameters.First(p => p.ParameterName == "p_result").Value);
            if (success == 0 || result.Rows.Count == 0)
                return null;

            var row = result.Rows[0];
            return new ProfilePicture
            {
                Id = Convert.ToInt32(row["id"]),
                UserId = Convert.ToInt32(row["user_id"]),
                FileName = row["file_name"].ToString() ?? string.Empty,
                FilePath = row["file_path"].ToString() ?? string.Empty,
                FileSize = Convert.ToInt64(row["file_size"]),
                MimeType = row["mime_type"].ToString() ?? string.Empty,
                StorageType = row["storage_type"].ToString() ?? "local",
                IsActive = Convert.ToBoolean(row["is_active"]),
                UploadedBy = Convert.ToInt32(row["uploaded_by"]),
                CreatedAt = Convert.ToDateTime(row["created_at"]),
                ModifiedAt = Convert.ToDateTime(row["modified_at"])
            };
        }

        public async Task<List<ProfilePictureResponse>> GetProfilePicturesByUserAsync(int userId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_requesting_user_id", userId),
                new MySqlParameter("p_target_user_id", userId),
                new MySqlParameter("p_result", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_message", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Output }
            };

            var result = await _context.ExecuteQueryAsync("sp_get_profile_picture", parameters);

            var success = Convert.ToInt32(parameters.First(p => p.ParameterName == "p_result").Value);
            if (success == 0)
                return new List<ProfilePictureResponse>();

            return result.Rows.Cast<DataRow>().Select(row => new ProfilePictureResponse
            {
                Id = Convert.ToInt32(row["id"]),
                UserId = Convert.ToInt32(row["user_id"]),
                UserName = row["user_name"].ToString() ?? string.Empty,
                UserType = row["user_type"].ToString() ?? string.Empty,
                FileName = row["file_name"].ToString() ?? string.Empty,
                FilePath = row["file_path"].ToString() ?? string.Empty,
                FileSize = Convert.ToInt64(row["file_size"]),
                MimeType = row["mime_type"].ToString() ?? string.Empty,
                StorageType = row["storage_type"].ToString() ?? "local",
                IsActive = Convert.ToBoolean(row["is_active"]),
                UploadedAt = Convert.ToDateTime(row["uploaded_at"]),
                UploadedByName = row["uploaded_by_name"].ToString() ?? string.Empty,
                CreatedAt = Convert.ToDateTime(row["created_at"]),
                CreatedByName = row["created_by_name"].ToString() ?? string.Empty,
                ModifiedAt = Convert.ToDateTime(row["modified_at"]),
                ModifiedByName = row["modified_by_name"].ToString() ?? string.Empty
            }).ToList();
        }

        public async Task<List<ProfilePictureResponse>> GetAllProfilePicturesAsync(string? userType, bool? isActive)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_admin_id", 0), // Will be set by service layer
                new MySqlParameter("p_user_type", userType ?? (object)DBNull.Value),
                new MySqlParameter("p_is_active", isActive ?? (object)DBNull.Value),
                new MySqlParameter("p_result", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_message", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Output }
            };

            var result = await _context.ExecuteQueryAsync("sp_list_profile_pictures", parameters);

            var success = Convert.ToInt32(parameters.First(p => p.ParameterName == "p_result").Value);
            if (success == 0)
                return new List<ProfilePictureResponse>();

            return result.Rows.Cast<DataRow>().Select(row => new ProfilePictureResponse
            {
                Id = Convert.ToInt32(row["id"]),
                UserId = Convert.ToInt32(row["user_id"]),
                UserName = row["user_name"].ToString() ?? string.Empty,
                UserType = row["user_type"].ToString() ?? string.Empty,
                FileName = row["file_name"].ToString() ?? string.Empty,
                FilePath = row["file_path"].ToString() ?? string.Empty,
                FileSize = Convert.ToInt64(row["file_size"]),
                MimeType = row["mime_type"].ToString() ?? string.Empty,
                StorageType = row["storage_type"].ToString() ?? "local",
                IsActive = Convert.ToBoolean(row["is_active"]),
                UploadedAt = Convert.ToDateTime(row["uploaded_at"]),
                UploadedByName = row["uploaded_by_name"].ToString() ?? string.Empty,
                CreatedAt = Convert.ToDateTime(row["created_at"]),
                CreatedByName = row["created_by_name"].ToString() ?? string.Empty,
                ModifiedAt = Convert.ToDateTime(row["modified_at"]),
                ModifiedByName = row["modified_by_name"].ToString() ?? string.Empty
            }).ToList();
        }
    }
}