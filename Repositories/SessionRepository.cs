using backend.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace backend.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly IDatabaseContext _context;

        public SessionRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<DataTable> CheckActiveSession(int userId, string jti)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_user_id", userId),
                new MySqlParameter("p_jti", jti)
            };

            return await _context.ExecuteQueryAsync("sp_CheckActiveSession", parameters);
        }

        public async Task<int> TerminateOtherSessions(int userId, string currentJti)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_user_id", userId),
                new MySqlParameter("p_current_jti", currentJti)
            };

            return await _context.ExecuteNonQueryAsync("sp_TerminateOtherSessions", parameters);
        }

        public async Task<DataTable> GetActiveSessions(int adminId, int? userId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_admin_id", adminId),
                new MySqlParameter("p_user_id", userId ?? (object)DBNull.Value)
            };

            return await _context.ExecuteQueryAsync("sp_GetActiveSessions", parameters);
        }
        public async Task<bool> IsSessionRevoked(string jti)
        {
            var parameters = new[]
            {
            new MySqlParameter("p_jti", jti)
        };

            var result = await _context.ExecuteQueryAsync("sp_CheckSessionRevoked", parameters);
            return result.Rows.Count > 0 && Convert.ToBoolean(result.Rows[0]["is_revoked"]);
        }
    }
}