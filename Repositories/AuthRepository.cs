using backend.Data;
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

        public async Task<DataTable> AdminLogin(string email, string passwordHash)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_email", email),
                new MySqlParameter("p_password_hash", passwordHash)
            };

            return await _context.ExecuteQueryAsync("sp_AdminLogin", parameters);
        }

        public async Task<DataTable> TeacherLogin(string email, string passwordHash)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_email", email),
                new MySqlParameter("p_password_hash", passwordHash)
            };

            return await _context.ExecuteQueryAsync("sp_TeacherLogin", parameters);
        }

        public async Task<DataTable> StudentLogin(string regNo, string passwordHash)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_reg_no", regNo),
                new MySqlParameter("p_password_hash", passwordHash)
            };

            return await _context.ExecuteQueryAsync("sp_StudentLogin", parameters);
        }

        public async Task<int> CreateUserSession(int userId, string sessionId, string jti, string deviceInfo, DateTime expiresAt)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_user_id", userId),
                new MySqlParameter("p_session_id", sessionId),
                new MySqlParameter("p_jti", jti),
                new MySqlParameter("p_device_info", deviceInfo),
                new MySqlParameter("p_expires_at", expiresAt)
            };

            return await _context.ExecuteNonQueryAsync("sp_CreateUserSession", parameters);
        }

        public async Task<bool> RevokeUserSession(int userId, string jti)
        {
            var parameters = new[]
            {
        new MySqlParameter("p_user_id", userId),
        new MySqlParameter("p_jti", jti)
    };

            try
            {
                var result = await _context.ExecuteQueryAsync("sp_RevokeUserSession", parameters);

                if (result.Rows.Count > 0)
                {
                    int sessionsRevoked = Convert.ToInt32(result.Rows[0]["sessions_revoked"]);
                    string message = result.Rows[0]["message"].ToString();

                   
                    return sessionsRevoked > 0;
                }

              
                return false;
            }
            catch (Exception ex)
            {
               
                return false;
            }
        }
        public async Task<bool> SessionExists(int userId, string jti)
        {
            var parameters = new[]
            {
        new MySqlParameter("p_user_id", userId),
        new MySqlParameter("p_jti", jti)
    };

            var result = await _context.ExecuteQueryAsync(
                "SELECT 1 FROM user_session WHERE user_id = @p_user_id AND jti = @p_jti AND is_revoked = FALSE AND expires_at > UTC_TIMESTAMP()",
                parameters);

            return result.Rows.Count > 0;
        }
        // In AuthRepository
        public async Task<bool> IsTokenRevoked(string jti)
        {
            var parameters = new[] { new MySqlParameter("p_jti", jti) };
            var result = await _context.ExecuteQueryAsync("sp_CheckSessionRevoked", parameters);
            return result.Rows.Count > 0 && Convert.ToBoolean(result.Rows[0]["is_revoked"]);
        }
    }
}