using backend.Dtos;
using backend.Repositories;
using System.Data;

namespace backend.Services
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;

        public SessionService(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public async Task TerminateOtherSessions(int userId, string currentJti)
        {
            await _sessionRepository.TerminateOtherSessions(userId, currentJti);
        }

        public async Task<IEnumerable<ActiveSessionResponse>> GetActiveSessions(int adminId, int? userId)
        {
            var result = await _sessionRepository.GetActiveSessions(adminId, userId);
            return result.Rows.Cast<DataRow>().Select(MapActiveSessionResponse);
        }

        private ActiveSessionResponse MapActiveSessionResponse(DataRow row)
        {
            return new ActiveSessionResponse
            {
                SessionId = Convert.ToInt32(row["id"]),
                UserId = Convert.ToInt32(row["user_id"]),
                UserName = row["name"].ToString() ?? string.Empty,
                Email = row["email"].ToString() ?? string.Empty,
                Profile = row["role"].ToString() ?? string.Empty,
                LoginAt = Convert.ToDateTime(row["login_at"]),
                ExpiresAt = Convert.ToDateTime(row["expires_at"]),
                DeviceInfo = row["device_info"]?.ToString() ?? "{}",
                LastActivity = Convert.ToDateTime(row["last_activity"])
            };
        }
    }
}