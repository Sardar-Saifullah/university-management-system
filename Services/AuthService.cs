using backend.Dtos;
using backend.Models;
using backend.Repositories;
using backend.Utilities;
using System.Data;

namespace backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtHandler _jwtHandler;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IPermissionRepository _permissionRepository;

        public AuthService(
            IAuthRepository authRepository,
            IJwtHandler jwtHandler,
            IPasswordHasher passwordHasher,
            IPermissionRepository permissionRepository)
        {
            _authRepository = authRepository;
            _jwtHandler = jwtHandler;
            _passwordHasher = passwordHasher;
            _permissionRepository = permissionRepository;
        }

        public async Task<AuthResponse> AdminLogin(LoginDto dto)
        {
            var passwordHash = _passwordHasher.HashPassword(dto.Password);
            var result = await _authRepository.AdminLogin(dto.Email, passwordHash);

            if (result.Rows.Count == 0)
                throw new UnauthorizedAccessException("Invalid credentials");

            var row = result.Rows[0];
            return await GenerateAuthResponse(row);
        }

        public async Task<AuthResponse> TeacherLogin(LoginDto dto)
        {
            var passwordHash = _passwordHasher.HashPassword(dto.Password);
            var result = await _authRepository.TeacherLogin(dto.Email, passwordHash);

            if (result.Rows.Count == 0)
                throw new UnauthorizedAccessException("Invalid credentials");

            var row = result.Rows[0];
            return await GenerateAuthResponse(row);
        }

        public async Task<AuthResponse> StudentLogin(StudentLoginDto dto)
        {
            var passwordHash = _passwordHasher.HashPassword(dto.Password);
            var result = await _authRepository.StudentLogin(dto.RegNo, passwordHash);

            if (result.Rows.Count == 0)
                throw new UnauthorizedAccessException("Invalid credentials");

            var row = result.Rows[0];
            return await GenerateAuthResponse(row);
        }

        public async Task Logout(int userId, string jti)
        {
            var revoked = await _authRepository.RevokeUserSession(userId, jti);
            if (!revoked)
            {
                throw new Exception("Failed to revoke session");
            }
        }

        private async Task<AuthResponse> GenerateAuthResponse(DataRow row)
        {
            var userId = Convert.ToInt32(row["id"]);
            var name = row["name"].ToString() ?? string.Empty;
            var email = row["email"].ToString() ?? string.Empty;
            var profileName = row["profile_name"].ToString() ?? string.Empty;
            var profilePicUrl = row["profile_pic_url"]?.ToString();

            var jti = Guid.NewGuid().ToString();
            var token = _jwtHandler.GenerateToken(userId, name, profileName, jti);

            // Get permissions using just the profile name
            var permissions = await _permissionRepository.GetProfilePermissions(profileName);

            await _authRepository.CreateUserSession(
                userId,
                Guid.NewGuid().ToString(),
                jti,
                "{}",
                DateTime.UtcNow.AddMinutes(30));

            return new AuthResponse
            {
                Id = userId,
                Name = name,
                Email = email,
                Profile = profileName,
                Token = token,
                ProfilePicUrl = profilePicUrl,
                Permissions = permissions.ToList()
            };
        }
    }
}