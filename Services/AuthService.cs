using backend.Dtos;
using backend.Models;
using backend.Repositories;
using backend.Utilities;
using System.Text.Json;
namespace backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtHandler _jwtHandler;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IPermissionService _permissionService;




        public AuthService(
            IAuthRepository authRepository,
            IJwtHandler jwtHandler,
            IPasswordHasher passwordHasher,
            IPermissionService permissionService)
        {
            _authRepository = authRepository;
            _jwtHandler = jwtHandler;
            _passwordHasher = passwordHasher;
            _permissionService = permissionService;
        }

        public async Task<AuthResponse> AdminLogin(LoginRequest request)
        {
            var userResponse = await _authRepository.AdminLogin(request.Email);
            if (userResponse == null) return null;

            // Fetch stored hash and verify here if not already included
            if (!await VerifyPasswordForUser(userResponse.Id, request.Password)) return null;

            return await GenerateAuthResponse(userResponse);
        }

        public async Task<AuthResponse> TeacherLogin(LoginRequest request)
        {
            var userResponse = await _authRepository.TeacherLogin(request.Email);
            if (userResponse == null) return null;

            if (!await VerifyPasswordForUser(userResponse.Id, request.Password)) return null;

            return await GenerateAuthResponse(userResponse);
        }

        public async Task<AuthResponse> StudentLogin(StudentLoginRequest request)
        {
            var userResponse = await _authRepository.StudentLogin(request.RegistrationNo);
            if (userResponse == null) return null;

            if (!await VerifyPasswordForUser(userResponse.Id, request.Password)) return null;

            return await GenerateAuthResponse(userResponse);
        }

        public async Task<UserSession> CreateSession(int userId, string deviceInfo, string jti)
        {
            var sessionId = Guid.NewGuid().ToString();
           
            var expiresAt = DateTime.UtcNow.AddHours(12);

            // Create proper JSON object for device info
            var deviceInfoJson = JsonSerializer.Serialize(new
            {
                device_type = "Web Browser",
                user_agent = deviceInfo,
                timestamp = DateTime.UtcNow.ToString(),
            });

            return await _authRepository.CreateUserSession(
                userId, sessionId, jti, deviceInfoJson, expiresAt);
        }

        public async Task<bool> ValidateSession(string jti)
        {
            return !await _authRepository.CheckSessionRevoked(jti);
        }

        public async Task<TerminateSessionsResponse> TerminateOtherSessions(int userId, string currentJti)
        {
            return await _authRepository.TerminateOtherSessions(userId, currentJti);
       
        }

        public async Task<IEnumerable<ActiveSessionResponse>> GetActiveSessions(int adminId, int? userId = null)
        {
            return await _authRepository.GetActiveSessions(adminId, userId);
           
        }

        public async Task<TerminateSessionsResponse> RevokeSession(int sessionId)
        {
            return await _authRepository.RevokeSession(sessionId);
           

        }

        private async Task<AuthResponse> GenerateAuthResponse(LoginUserResponse userResponse)
        {
            var jti = Guid.NewGuid().ToString();
            var token = _jwtHandler.GenerateToken(userResponse.Id, userResponse.Name, userResponse.ProfileName, jti);
            var session = await CreateSession(userResponse.Id, "Web Browser", jti);
            var permissions = await GetProfilePermissions(userResponse.ProfileName);

            // Debug: Log permissions count and details
            Console.WriteLine($"Generated auth response for {userResponse.Name} ({userResponse.ProfileName})");
            Console.WriteLine($"Permissions count: {permissions?.Count}");

            if (permissions != null)
            {
                foreach (var perm in permissions)
                {
                    Console.WriteLine($"Permission: {perm.ActivityName} (Read: {perm.CanRead}, Write: {perm.CanCreate})");
                }
            }

            return new AuthResponse
            {
                Token = token,
                Id = userResponse.Id,
                Name = userResponse.Name,
                Email = userResponse.Email,
                Profile = userResponse.ProfileName,
                ProfilePicUrl = userResponse.ProfilePicUrl,
                ProfilePicType = userResponse.ProfilePictureType,
                SessionId = session.SessionId,
                ExpiresAt = session.ExpiresAt,
                RegistrationNo = userResponse.ProfileName == "Student" ? userResponse.RegistrationNo : null,
                ProgramName = userResponse.ProfileName == "Student" ? userResponse.ProgramName : null,
                Designation = userResponse.ProfileName == "Teacher" ? userResponse.Designation : null,
                Permissions = permissions ?? new List<PermissionResponse>()
            };
        }

        private async Task<List<PermissionResponse>> GetProfilePermissions(string profileName)
        {
            // You'll need to inject IPermissionService or IPermissionRepository

            return await _permissionService.GetProfilePermissions(profileName);
        }

        private async Task<bool> VerifyPasswordForUser(int userId, string providedPassword)
        {
            // Load stored hash from DB using a simple scalar proc
            var storedHash = await _authRepository.GetUserPasswordHash(userId);
            return _passwordHasher.VerifyPassword(storedHash, providedPassword);
        }
    }
}