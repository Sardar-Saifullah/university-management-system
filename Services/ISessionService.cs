using backend.Dtos;
namespace backend.Services
{
    public interface ISessionService
    {
        Task TerminateOtherSessions(int userId, string currentJti);
        Task<IEnumerable<ActiveSessionResponse>> GetActiveSessions(int adminId, int? userId);
    }
}
