using System.Data;

namespace backend.Repositories
{
    public interface ISessionRepository
    {
        Task<DataTable> CheckActiveSession(int userId, string jti);
        Task<int> TerminateOtherSessions(int userId, string currentJti);
        Task<DataTable> GetActiveSessions(int adminId, int? userId);
        Task<bool> IsSessionRevoked(string jti);
    }
}
