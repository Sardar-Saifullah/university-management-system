// Controllers/PrerequisiteController.cs
using backend.Filters;
using backend.Dtos;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PrerequisiteController : ControllerBase
    {
        private readonly IPrerequisiteService _prerequisiteService;

        public PrerequisiteController(IPrerequisiteService prerequisiteService)
        {
            _prerequisiteService = prerequisiteService;
        }

        [HttpPost]
        [PermissionRequired("prerequisite", "create", "prerequisite")]
        public async Task<IActionResult> AddPrerequisite(AddPrerequisiteDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("id")?.Value);
            await _prerequisiteService.AddPrerequisite(dto, userId);
            return Ok(new { message = "Prerequisite added successfully" });
        }

        [HttpDelete]
        [PermissionRequired("prerequisite", "delete", "prerequisite")]
        public async Task<IActionResult> RemovePrerequisite(RemovePrerequisiteDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("id")?.Value);
            await _prerequisiteService.RemovePrerequisite(dto, userId);
            return Ok(new { message = "Prerequisite removed successfully" });
        }

        [HttpGet("{courseId}")]
        [PermissionRequired("prerequisite", "read", "prerequisite")]
        public async Task<IActionResult> GetPrerequisites(int courseId, [FromQuery] bool includeInactive = false)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("id")?.Value);
            var prerequisites = await _prerequisiteService.GetPrerequisites(courseId, includeInactive, userId);
            return Ok(prerequisites);
        }

        [HttpPost("validate")]
        [PermissionRequired("prerequisite", "read", "prerequisite_validation")]
        public async Task<IActionResult> ValidatePrerequisites(ValidatePrerequisitesDto dto)
        {
            var result = await _prerequisiteService.ValidatePrerequisites(dto);
            return Ok(result);
        }
    }
}