using System.Data;

namespace backend.Repositories
{
    public interface IAuthRepository
    {
        Task<DataTable> AdminLogin(string email, string passwordHash);
        Task<DataTable> TeacherLogin(string email, string passwordHash);
        Task<DataTable> StudentLogin(string regNo, string passwordHash);
        Task<int> CreateUserSession(int userId, string sessionId, string jti, string deviceInfo, DateTime expiresAt);
        Task<bool> RevokeUserSession(int userId, string jti);
        Task<bool> SessionExists(int userId, string jti);

        Task<bool> IsTokenRevoked(string jti);

    }
}
