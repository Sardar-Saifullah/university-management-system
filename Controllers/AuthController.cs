using backend.Dtos;
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
        public async Task<ActionResult<AuthResponse>> AdminLogin(LoginDto dto)
        {
            var result = await _authService.AdminLogin(dto);
            return Ok(result);
        }

        [HttpPost("teacher/login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> TeacherLogin(LoginDto dto)
        {
            var result = await _authService.TeacherLogin(dto);
            return Ok(result);
        }

        [HttpPost("student/login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> StudentLogin(StudentLoginDto dto)
        {
            var result = await _authService.StudentLogin(dto);
            return Ok(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var jti = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");

            if (string.IsNullOrEmpty(jti) || userId == 0)
            {
                return BadRequest("Invalid token claims");
            }

            try
            {
                await _authService.Logout(userId, jti);
                return Ok(new { message = "Successfully logged out" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}