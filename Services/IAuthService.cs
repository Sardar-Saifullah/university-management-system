using backend.Dtos;
namespace backend.Services
{
    public interface IAuthService
    {
        Task<AuthResponse>AdminLogin(LoginDto dto);
        Task<AuthResponse>TeacherLogin(LoginDto dto);
        Task<AuthResponse>StudentLogin(StudentLoginDto dto);
        Task Logout(int userId, string jti);
    }
}
