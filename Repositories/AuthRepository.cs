using backend.Data;
using backend.Dtos;
using backend.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace backend.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IDatabaseContext _context;

        public AuthRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<LoginUserResponse> AdminLogin(string email)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_email", email)
            };

            try
            {
                var result = await _context.ExecuteQueryAsync("sp_AdminLogin", parameters);
                if (result.Rows.Count == 0) return null;

                return MapToLoginResponse(result.Rows[0]);
            }
            catch (MySqlException ex)
            {
                // Treat MySQL SIGNAL (ER_SIGNAL_EXCEPTION = 1644) as invalid credentials
                if (ex.Number == 1644) return null;
                throw;
            }
        }

        

        public async Task<LoginUserResponse> TeacherLogin(string email)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_email", email)
            };

            try
            {
                var result = await _context.ExecuteQueryAsync("sp_TeacherLogin", parameters);
                if (result.Rows.Count == 0) return null;

                return MapToLoginResponse(result.Rows[0]);
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1644) return null;
                throw;
            }
        }

        public async Task<LoginUserResponse> StudentLogin(string RegistrationNo)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_reg_no", RegistrationNo)
            };

            try
            {
                var result = await _context.ExecuteQueryAsync("sp_StudentLogin", parameters);
                if (result.Rows.Count == 0) return null;

                return MapToLoginResponse(result.Rows[0]);
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1644) return null;
                throw;
            }
        }

        public async Task<UserSession> CreateUserSession(int userId, string sessionId, string jti, string deviceInfo, DateTime expiresAt)
        {
            Console.WriteLine($"Device info being sent: {deviceInfo}");
            var parameters = new[]
            {
                new MySqlParameter("p_user_id", userId),
                new MySqlParameter("p_session_id", sessionId),
                new MySqlParameter("p_jti", jti),
                new MySqlParameter("p_device_info", deviceInfo),
                new MySqlParameter("p_expires_at", expiresAt)
            };

            try
            {
                var result = await _context.ExecuteQueryAsync("sp_CreateUserSession", parameters);
                if (result.Rows.Count == 0) return null;

                return MapToUserSession(result.Rows[0]);
            }
            catch (MySqlException ex)
            {
                // Log the error for debugging
                Console.WriteLine($"MySQL Error creating session: {ex.Message}");
                throw;
            }
        }
      
        public async Task<bool> CheckSessionRevoked(string jti)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_jti", jti)
            };

            var result = await _context.ExecuteQueryAsync("sp_CheckSessionRevoked", parameters);
            return result.Rows.Count > 0 && Convert.ToBoolean(result.Rows[0]["is_revoked"]);
        }

        public async Task<TerminateSessionsResponse> TerminateOtherSessions(int userId, string currentJti)
        {
            var parameters = new[]
            {
        new MySqlParameter("p_user_id", userId),
        new MySqlParameter("p_current_jti", currentJti)
    };

            var result = await _context.ExecuteQueryAsync("sp_TerminateOtherSessions", parameters);

            if (result.Rows.Count > 0)
            {
                int sessionsTerminated = Convert.ToInt32(result.Rows[0]["sessions_terminated"]);
                string message = $"Successfully terminated {sessionsTerminated} other session(s).";
                return new TerminateSessionsResponse(true, message, sessionsTerminated);
            }
            // If no rows were found to terminate, it's still a success, just with count 0.
            return new TerminateSessionsResponse(true, "No other active sessions were found to terminate.", 0);
        }

        public async Task<IEnumerable<ActiveSessionResponse>> GetActiveSessions(int adminId, int? userId = null)
        {
            var parameters = new[]
            {
        new MySqlParameter("p_admin_id", adminId),
        new MySqlParameter("p_user_id", userId ?? (object)DBNull.Value)
    };

            var result = await _context.ExecuteQueryAsync("sp_GetActiveSessions", parameters);
            return result.Rows.Cast<DataRow>().Select(MapToActiveSessionResponse).ToList();
        }

        public async Task<TerminateSessionsResponse> RevokeSession(int sessionId)
        {
            var parameters = new[] { new MySqlParameter("p_session_id", sessionId) };

            var result = await _context.ExecuteQueryAsync("sp_RevokeUserSession", parameters);

            if (result.Rows.Count > 0)
            {
                int sessionsRevoked = Convert.ToInt32(result.Rows[0]["sessions_revoked"]);
                string message = result.Rows[0]["message"].ToString();
                bool success = sessionsRevoked > 0;

                return new TerminateSessionsResponse(success, message, sessionsRevoked);
            }
            return new TerminateSessionsResponse(false, "An error occurred while revoking the session.");
        }

        public async Task<string> GetUserPasswordHash(int userId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_user_id", userId)
            };

            // Expect a single-row, single-column table with password_hash
            var result = await _context.ExecuteQueryAsync("sp_GetUserPasswordHash", parameters);
            if (result.Rows.Count == 0) return string.Empty;
            return result.Rows[0]["password_hash"].ToString();
        }

        private LoginUserResponse MapToLoginResponse(DataRow row)
        {
            var profileName = row["profile_name"].ToString();
            var response = new LoginUserResponse
            {
                Id = Convert.ToInt32(row["id"]),
                Name = row["name"].ToString(),
                Email = row["email"].ToString(),
                ProfileName = row["profile_name"].ToString()
            };

            // Handle optional columns safely
            if (row.Table.Columns.Contains("profile_pic_url") && row["profile_pic_url"] != DBNull.Value)
            {
                response.ProfilePicUrl = row["profile_pic_url"].ToString();
            }

            if (row.Table.Columns.Contains("profile_picture_type") && row["profile_picture_type"] != DBNull.Value)
            {
                response.ProfilePictureType = row["profile_picture_type"].ToString();
            }


            // Only set registrationNo and programName for students
            if (profileName.Equals("Student", StringComparison.OrdinalIgnoreCase))
            {
                if (row.Table.Columns.Contains("reg_no") && row["reg_no"] != DBNull.Value)
                {
                    response.RegistrationNo = row["reg_no"].ToString();
                }

                if (row.Table.Columns.Contains("program_name") && row["program_name"] != DBNull.Value)
                {
                    response.ProgramName = row["program_name"].ToString();
                }
            }

            // Only set designation for teachers
            if (profileName.Equals("Teacher", StringComparison.OrdinalIgnoreCase))
            {
                if (row.Table.Columns.Contains("designation") && row["designation"] != DBNull.Value)
                {
                    response.Designation = row["designation"].ToString();
                }
            }

            if (row.Table.Columns.Contains("reg_no") && row["reg_no"] != DBNull.Value)
            {
                response.RegistrationNo = row["reg_no"].ToString();
            }

            if (row.Table.Columns.Contains("program_name") && row["program_name"] != DBNull.Value)
            {
                response.ProgramName = row["program_name"].ToString();
            }

            if (row.Table.Columns.Contains("designation") && row["designation"] != DBNull.Value)
            {
                response.Designation = row["designation"].ToString();
            }

            return response;
        }

        private UserSession MapToUserSession(DataRow row)
        {
            return new UserSession
            {
                Id = Convert.ToInt32(row["id"]),
                UserId = Convert.ToInt32(row["user_id"]),
                SessionId = row["session_id"].ToString(),
                Jti = row["jti"].ToString(),
                DeviceInfo = row["device_info"].ToString(),
                LoginAt = Convert.ToDateTime(row["login_at"]),
                ExpiresAt = Convert.ToDateTime(row["expires_at"]),
                IsRevoked = Convert.ToBoolean(row["is_revoked"]),
                RevokedAt = row["revoked_at"] != DBNull.Value ? Convert.ToDateTime(row["revoked_at"]) : (DateTime?)null,
                LastActivity = Convert.ToDateTime(row["last_activity"])
            };
        }

        private ActiveSessionResponse MapToActiveSessionResponse(DataRow row)
        {
            return new ActiveSessionResponse
            {
                SessionId = Convert.ToInt32(row["id"]),
                UserId = Convert.ToInt32(row["user_id"]),
                UserName = row["name"].ToString(),
                Email = row["email"].ToString(),
                Profile = row["role"].ToString(),
                LoginAt = Convert.ToDateTime(row["login_at"]),
                ExpiresAt = Convert.ToDateTime(row["expires_at"]),
                DeviceInfo = row["device_info"].ToString(),
                LastActivity = Convert.ToDateTime(row["last_activity"])
            };
        }
    }
}