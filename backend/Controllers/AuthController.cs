using backend.Dtos;
using backend.Filters;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("admin/login")]
        [AllowAnonymous]
        public async Task<IActionResult> AdminLogin(LoginRequest request)
        {
            var response = await _authService.AdminLogin(request);
            if (response == null) return Unauthorized("Invalid credentials");
            return Ok(response);
        }

        [HttpPost("teacher/login")]
        [AllowAnonymous]
        public async Task<IActionResult> TeacherLogin(LoginRequest request)
        {
            var response = await _authService.TeacherLogin(request);
            if (response == null) return Unauthorized("Invalid credentials");
            return Ok(response);
        }

        [HttpPost("student/login")]
        [AllowAnonymous]
        public async Task<IActionResult> StudentLogin(StudentLoginRequest request)
        {
            var response = await _authService.StudentLogin(request);
            if (response == null) return Unauthorized("Invalid credentials");
            return Ok(response);
        }

        [HttpGet("sessions")]
        [PermissionRequired("AuthController", "read", "user_sessions")]
        public async Task<IActionResult> GetActiveSessions(int? userId)
        {
            var currentUserId = int.Parse(User.FindFirst("id").Value);
            var currentProfile = User.FindFirst("profile").Value;

            // Admin can view all sessions, others can only view their own
            if (currentProfile != "admin")
            {
                userId = currentUserId;
            }
            var sessions = await _authService.GetActiveSessions(currentUserId, userId);
            return Ok(sessions);
        }

        [HttpPost("sessions/revoke")]
        [PermissionRequired("AuthController", "delete", "user_sessions")]
        public async Task<IActionResult> RevokeSession([FromBody] RevokeSessionRequest request)
        {
            var result = await _authService.RevokeSession(request.SessionId);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("sessions/terminate-others")]
        [PermissionRequired("AuthController", "delete", "user_sessions")]
        public async Task<IActionResult> TerminateOtherSessions()
        {
            var currentUserId = int.Parse(User.FindFirst("id").Value);
            var currentJti = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            var result = await _authService.TerminateOtherSessions(currentUserId, currentJti);
            return Ok(result);
        }
    }
}
