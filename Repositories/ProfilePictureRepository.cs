using backend.Data;
using backend.Models;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading.Tasks;

namespace backend.Repositories
{
    public class ProfilePictureRepository : IProfilePictureRepository
    {
        private readonly IDatabaseContext _context;

        public ProfilePictureRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, int PictureId, string Message)> CreateProfilePictureAsync(int adminId, ProfilePicture picture)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_admin_id", adminId),
                new MySqlParameter("p_user_id", picture.UserId),
                new MySqlParameter("p_file_name", picture.FileName),
                new MySqlParameter("p_file_path", picture.FilePath),
                new MySqlParameter("p_file_size", picture.FileSize),
                new MySqlParameter("p_mime_type", picture.MimeType),
                new MySqlParameter("p_storage_type", picture.StorageType),
                new MySqlParameter("p_result", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_message", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_picture_id", MySqlDbType.Int32) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_create_profile_picture", parameters);

            var success = Convert.ToInt32(parameters[7].Value) == 1;
            var pictureId = success ? Convert.ToInt32(parameters[9].Value) : 0;
            var message = parameters[8].Value?.ToString() ?? "Unknown error";

            return (success, pictureId, message);
        }

        public async Task<(bool Success, ProfilePictureResponse Data, string Message)> GetProfilePictureAsync(int requestingUserId, int targetUserId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_requesting_user_id", requestingUserId),
                new MySqlParameter("p_target_user_id", targetUserId),
                new MySqlParameter("p_result", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_message", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Output }
            };

            try
            {
                var dataTable = await _context.ExecuteQueryAsync("sp_get_profile_picture", parameters);

                var success = Convert.ToInt32(parameters[2].Value) == 1;
                var message = parameters[3].Value?.ToString() ?? "Unknown error";

                if (!success || dataTable.Rows.Count == 0)
                    return (false, null, message);

                var row = dataTable.Rows[0];
                var response = new ProfilePictureResponse
                {
                    Id = Convert.ToInt32(row["id"]),
                    UserId = Convert.ToInt32(row["user_id"]),
                    UserName = row["user_name"].ToString(),
                    UserType = row["user_type"].ToString(),
                    FileName = row["file_name"].ToString(),
                    FilePath = row["file_path"].ToString(),
                    FileSize = Convert.ToInt64(row["file_size"]),
                    MimeType = row["mime_type"].ToString(),
                    StorageType = row["storage_type"].ToString(),
                    IsActive = Convert.ToBoolean(row["is_active"]),
                    IsCurrentActive = Convert.ToBoolean(row["is_current_active"]),
                    UploadedAt = Convert.ToDateTime(row["uploaded_at"]),
                    UploadedByName = row["uploaded_by_name"]?.ToString() ?? string.Empty,
                    CreatedAt = Convert.ToDateTime(row["created_at"]),
                    CreatedByName = row["created_by_name"]?.ToString() ?? string.Empty,
                    ModifiedAt = Convert.ToDateTime(row["modified_at"]),
                    ModifiedByName = row["modified_by_name"]?.ToString() ?? string.Empty
                };

                return (true, response, message);
            }
            catch (Exception ex)
            {
                return (false, null, $"Database error: {ex.Message}");
            }
        }

        public async Task<(bool Success, ProfilePictureResponse Data, string Message)> GetMyProfilePictureAsync(int userId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_user_id", userId),
                new MySqlParameter("p_result", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_message", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Output }
            };

            try
            {
                var dataTable = await _context.ExecuteQueryAsync("sp_get_my_profile_picture", parameters);

                var success = Convert.ToInt32(parameters[1].Value) == 1;
                var message = parameters[2].Value?.ToString() ?? "Unknown error";

                if (!success || dataTable.Rows.Count == 0)
                    return (false, null, message);

                var row = dataTable.Rows[0];
                var response = new ProfilePictureResponse
                {
                    Id = Convert.ToInt32(row["id"]),
                    UserId = Convert.ToInt32(row["user_id"]),
                    UserName = row["user_name"].ToString(),
                    UserType = row["user_type"].ToString(),
                    FileName = row["file_name"].ToString(),
                    FilePath = row["file_path"].ToString(),
                    FileSize = Convert.ToInt64(row["file_size"]),
                    MimeType = row["mime_type"].ToString(),
                    StorageType = row["storage_type"].ToString(),
                    IsActive = Convert.ToBoolean(row["is_active"]),
                    IsCurrentActive = Convert.ToBoolean(row["is_current_active"]),
                    UploadedAt = Convert.ToDateTime(row["uploaded_at"])
                };

                return (true, response, message);
            }
            catch (Exception ex)
            {
                return (false, null, $"Database error: {ex.Message}");
            }
        }

        public async Task<(bool Success, string FilePath, string Message)> DeleteProfilePictureAsync(int adminId, int pictureId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_admin_id", adminId),
                new MySqlParameter("p_picture_id", pictureId),
                new MySqlParameter("p_result", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_message", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Output }
            };

            string filePath = null;

            try
            {
                var dataTable = await _context.ExecuteQueryAsync("sp_delete_profile_picture", parameters);

                var success = Convert.ToInt32(parameters[2].Value) == 1;
                var message = parameters[3].Value?.ToString() ?? "Unknown error";

                if (success && dataTable.Rows.Count > 0)
                {
                    filePath = dataTable.Rows[0]["file_path_to_delete"]?.ToString();
                }

                return (success, filePath, message);
            }
            catch (Exception ex)
            {
                return (false, null, $"Database error: {ex.Message}");
            }
        }
    }
}