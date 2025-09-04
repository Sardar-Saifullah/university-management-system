// Controllers/ProfileController.cs
using backend.Dtos;
using backend.Filters;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        // Admin Profile Endpoints
        [HttpGet("admin")]
        [PermissionRequired("profile", "read", "admin_profile")]
        public async Task<ActionResult<AdminProfileDto>> GetAdminProfile()
        {
            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("id")?.Value);
            var profile = await _profileService.GetAdminProfile(adminId);

            if (profile == null)
                return NotFound("Admin profile not found");

            return Ok(profile);
        }

        [HttpPut("admin")]
        [PermissionRequired("profile", "update", "admin_profile")]
        public async Task<IActionResult> UpdateAdminProfile([FromBody] AdminOwnProfileUpdateDto updateDto)
        {
            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("id")?.Value);
            var result = await _profileService.UpdateAdminProfile(adminId, updateDto);

            if (!result)
                return BadRequest("Failed to update admin profile");

            return NoContent();
        }

        // Teacher Profile Endpoints
        [HttpGet("teacher")]
        [PermissionRequired("profile", "read", "teacher_profile")]
        public async Task<ActionResult<TeacherProfileDto>> GetTeacherProfile()
        {
            var teacherId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("id")?.Value);
            var profile = await _profileService.GetTeacherProfile(teacherId);

            if (profile == null)
                return NotFound("Teacher profile not found");

            return Ok(profile);
        }

        [HttpPut("teacher")]
        [PermissionRequired("profile", "update", "teacher_profile")]
        public async Task<IActionResult> UpdateTeacherProfile([FromBody] TeacherOwnProfileUpdateDto updateDto)
        {
            var teacherId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("id")?.Value);
            var result = await _profileService.UpdateTeacherProfile(teacherId, updateDto);

            if (!result)
                return BadRequest("Failed to update teacher profile");

            return NoContent();
        }

        [HttpPut("teacher/qualifications")]
        [PermissionRequired("profile", "update", "teacher_qualifications")]
        public async Task<IActionResult> UpdateTeacherQualifications([FromBody] TeacherQualificationUpdateDto updateDto)
        {
            var teacherId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("id")?.Value);
            var result = await _profileService.UpdateTeacherQualifications(teacherId, updateDto);

            if (!result)
                return BadRequest("Failed to update teacher qualifications");

            return NoContent();
        }

        // Student Profile Endpoints
        [HttpGet("student")]
        [PermissionRequired("profile", "read", "student_profile")]
        public async Task<ActionResult<StudentProfileDto>> GetStudentProfile()
        {
            var studentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("id")?.Value);
            var profile = await _profileService.GetStudentProfile(studentId);

            if (profile == null)
                return NotFound("Student profile not found");

            return Ok(profile);
        }

        [HttpPut("student")]
        [PermissionRequired("profile", "update", "student_profile")]
        public async Task<IActionResult> UpdateStudentProfile([FromBody] StudentOwnProfileUpdateDto updateDto)
        {
            var studentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("id")?.Value);
            var result = await _profileService.UpdateStudentProfile(studentId, updateDto);

            if (!result)
                return BadRequest("Failed to update student profile");

            return NoContent();
        }
    }
}