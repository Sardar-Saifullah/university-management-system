using backend.Dtos;
using backend.Models;

namespace backend.Repositories
{
    public interface IAuthRepository
    {
        Task<LoginUserResponse> AdminLogin(string email);

       
        Task<LoginUserResponse> TeacherLogin(string email);
        Task<LoginUserResponse> StudentLogin(string RegistrationNo);
        Task<UserSession> CreateUserSession(int userId, string sessionId, string jti, string deviceInfo, DateTime expiresAt);
        Task<bool> CheckSessionRevoked(string jti);
        Task<TerminateSessionsResponse> TerminateOtherSessions(int userId, string currentJti);
        Task<IEnumerable<ActiveSessionResponse>> GetActiveSessions(int adminId, int? userId = null);
        Task<TerminateSessionsResponse> RevokeSession(int sessionId);
        Task<string> GetUserPasswordHash(int userId);
    }
}