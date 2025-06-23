using backend.Dtos;
using backend.Filters;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
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

        [HttpGet("admin")]
        [PermissionRequired("admin_view_profile")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AdminProfileResponse>> GetAdminProfile()
        {
            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var profile = await _profileService.GetAdminProfileAsync(adminId);
            return Ok(profile);
        }

        [HttpPut("admin")]
        [PermissionRequired("admin_update_profile")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AdminProfileResponse>> UpdateAdminProfile(
     [FromForm] AdminProfileUpdateWithFileDto dto)
        {
            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            // MODIFIED: Explicit contact handling instructions
            var updateDto = new AdminProfileUpdateDto
            {
                Name = dto.Name,
                Contact = dto.Contact // NULL, "", or value
            };

            var updatedProfile = await _profileService.UpdateAdminProfileAsync(
                adminId, updateDto, dto.ProfilePic);

            return Ok(updatedProfile);
        }

        [HttpDelete("admin/picture")]
        [PermissionRequired("admin_delete_profile_pic")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteAdminProfilePicture()
        {
            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var success = await _profileService.DeleteAdminProfilePicAsync(adminId);
            return success ? NoContent() : BadRequest("Failed to delete profile picture");
        }

        [HttpGet("teacher")]
        [PermissionRequired("teacher_view_profile")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<TeacherProfileResponse>> GetTeacherProfile()
        {
            var teacherId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var profile = await _profileService.GetTeacherProfileAsync(teacherId);
            return Ok(profile);
        }

        [HttpPut("teacher")]
        [PermissionRequired("teacher_update_profile")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<TeacherProfileResponse>> UpdateTeacherProfile(
           [FromForm] TeacherProfileUpdateDto dto,
           [FromForm] IFormFile? profilePic)
        {
            var teacherId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var updatedProfile = await _profileService.UpdateTeacherProfileAsync(teacherId, dto, profilePic);
            return Ok(updatedProfile);
        }

        [HttpPut("teacher/qualifications")]
        [PermissionRequired("teacher_update_qualifications")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<TeacherProfileResponse>> UpdateTeacherQualifications([FromBody] TeacherQualificationsUpdateDto dto)
        {
            var teacherId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var updatedProfile = await _profileService.UpdateTeacherQualificationsAsync(teacherId, dto);
            return Ok(updatedProfile);
        }

        [HttpDelete("teacher/picture")]
        [PermissionRequired("teacher_delete_profile_pic")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult> DeleteTeacherProfilePicture()
        {
            var teacherId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var success = await _profileService.DeleteTeacherProfilePicAsync(teacherId);
            return success ? NoContent() : BadRequest("Failed to delete profile picture");
        }

        [HttpGet("student")]
        [PermissionRequired("student_view_profile")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<StudentProfileResponse>> GetStudentProfile()
        {
            var studentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var profile = await _profileService.GetStudentProfileAsync(studentId);
            return Ok(profile);
        }

        [HttpPut("student")]
        [PermissionRequired("student_update_profile")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<StudentProfileResponse>> UpdateStudentProfile(
           [FromForm] StudentProfileUpdateDto dto,
           [FromForm] IFormFile? profilePic)
        {
            var studentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var updatedProfile = await _profileService.UpdateStudentProfileAsync(studentId, dto, profilePic);
            return Ok(updatedProfile);
        }

        [HttpDelete("student/picture")]
        [PermissionRequired("student_delete_profile_pic")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> DeleteStudentProfilePicture()
        {
            var studentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var success = await _profileService.DeleteStudentProfilePicAsync(studentId);
            return success ? NoContent() : BadRequest("Failed to delete profile picture");
        }
    }
}