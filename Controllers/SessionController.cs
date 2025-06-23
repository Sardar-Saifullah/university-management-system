// SessionsController.cs
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
    [Authorize]
    public class SessionsController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpPost("terminate-others")]
        public async Task<IActionResult> TerminateOtherSessions()
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var jti = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

            if (userId == 0 || string.IsNullOrEmpty(jti))
                return BadRequest("Invalid user");

            await _sessionService.TerminateOtherSessions(userId, jti);
            return Ok();
        }

        [HttpGet]
        [PermissionRequired("Manage Active Sessions")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ActiveSessionResponse>>> GetActiveSessions(int? userId)
        {
            var adminId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var result = await _sessionService.GetActiveSessions(adminId, userId);
            return Ok(result);
        }
    }
}
