using backend.Dtos;
using backend.Models;

namespace backend.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> AdminLogin(LoginRequest request);
        Task<AuthResponse> TeacherLogin(LoginRequest request);
        Task<AuthResponse> StudentLogin(StudentLoginRequest request);
        Task<UserSession> CreateSession(int userId, string deviceInfo,string jti);
        Task<bool> ValidateSession(string jti);
        Task<TerminateSessionsResponse> TerminateOtherSessions(int userId, string currentJti);
        Task<IEnumerable<ActiveSessionResponse>> GetActiveSessions(int adminId, int? userId = null);
        Task<TerminateSessionsResponse> RevokeSession(int sessionId);
    }
}