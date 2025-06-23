using backend.Dtos;
using backend.Filters;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register/student")]
        [PermissionRequired("Register Student")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserResponse>> RegisterStudent(StudentRegistrationDto dto)
        {
            var createdBy = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var result = await _userService.RegisterStudent(dto, createdBy);
            return Ok(result);
        }

        [HttpPost("register/teacher")]
        [PermissionRequired("Register Teacher")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserResponse>> RegisterTeacher(TeacherRegistrationDto dto)
        {
            var createdBy = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var result = await _userService.RegisterTeacher(dto, createdBy);
            return Ok(result);
        }

        [HttpPost("register/admin")]
        [PermissionRequired("Register Admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserResponse>> RegisterAdmin(AdminRegistrationDto dto)
        {
            var createdBy = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var result = await _userService.RegisterAdmin(dto, createdBy);
            return Ok(result);
        }

        [HttpGet("profile")]
        public async Task<ActionResult<UserProfileResponse>> GetUserProfile()
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var result = await _userService.GetUserProfile(userId);
            return Ok(result);
        }


        [HttpGet("students/{regNo}")]
        [PermissionRequired("get_student_details")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<StudentDetailsResponse>> GetStudentByRegNo(string regNo)
        {
            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _userService.GetStudentByRegNo(adminId, regNo);
            return Ok(result);
        }

        [HttpGet("teachers")]
        [PermissionRequired("get_teacher_details")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<TeacherDetailsResponse>>> GetTeacherByDetails(
     [FromQuery] AdminGetTeacherByDetailsDto dto)
        {
            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _userService.GetTeacherByDetails(
                adminId, dto.Name, dto.DepId, dto.Designation);
            return Ok(result);
        }

        [HttpPut("students")]
        [PermissionRequired("update_student_profile")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateStudent([FromBody] AdminUpdateStudentDto dto)
        {
            // Convert empty string to null
            if (string.IsNullOrWhiteSpace(dto.ProfilePicUrl))
            {
                dto.ProfilePicUrl = null;
            }
            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var success = await _userService.UpdateStudent(adminId, dto);
            return success ? Ok() : BadRequest("Failed to update student");
        }

        [HttpPut("teachers")]
        [PermissionRequired("update_teacher_profile")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateTeacher([FromBody] AdminUpdateTeacherDto dto)
        {
            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var success = await _userService.UpdateTeacher(adminId, dto);
            return success ? Ok() : BadRequest("Failed to update teacher");
        }

        [HttpDelete("{userId}")]
        [PermissionRequired("delete_user")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> SoftDeleteUser(int userId)
        {
            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var (success, message) = await _userService.SoftDeleteUser(adminId, userId);

            return success
                ? Ok(new { Success = true, Message = message })
                : BadRequest(new { Success = false, Message = message });
        }

        [HttpGet("count")]
        [PermissionRequired("count_users")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> CountActiveUsers([FromQuery] AdminCountUsersDto dto)
        {
            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var count = await _userService.CountActiveUsers(adminId, dto.ProfileName);
            return Ok(count);
        }

        [HttpGet]
        [PermissionRequired("list_users")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<UserResponse>>> GetAllUsersByProfile(
      [FromQuery]AdminGetAllUsersDto dto)
        {
            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _userService.GetAllUsersByProfile(adminId, dto);
            return Ok(result);
        }
    }
}