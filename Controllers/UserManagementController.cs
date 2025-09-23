using backend.Dtos;
using backend.Filters;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;

        public UserManagementController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        [HttpGet("users")]
        [PermissionRequired("UserManagement", "read", "users")]
        public async Task<ActionResult<PaginationResponseDto<UserResponseDto>>> GetUsers(
            [FromQuery] PaginationRequestDto request,
            [FromQuery] int? profileId = null,
            [FromQuery] bool? isActive = null)
        {
            try
            {
                var adminId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                var result = await _userManagementService.GetUsersAsync(adminId, request, profileId, isActive);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching users", error = ex.Message });
            }
        }

        [HttpGet("users/{userId}")]
        [PermissionRequired("UserManagement", "read", "user")]
        public async Task<ActionResult<UserResponseDto>> GetUser(int userId)
        {
            try
            {
                var adminId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                var user = await _userManagementService.GetUserByIdAsync(adminId, userId);

                if (user == null)
                    return NotFound(new { message = "User not found" });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching user", error = ex.Message });
            }
        }

        [HttpPut("users/{userId}")]
        [PermissionRequired("UserManagement", "update", "user")]
        public async Task<ActionResult<UserResponseDto>> UpdateUser(int userId, [FromBody] UserUpdateDto updateDto)
        {
            try
            {
                var adminId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                var success = await _userManagementService.UpdateUserAsync(adminId, userId, updateDto);

                if (!success)
                    return NotFound(new { message = "User not found or no changes made" });

                var updatedUser = await _userManagementService.GetUserByIdAsync(adminId, userId);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating user", error = ex.Message });
            }
        }

        [HttpDelete("users/{userId}")]
        [PermissionRequired("UserManagement", "delete", "user")]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            try
            {
                var adminId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                var success = await _userManagementService.DeleteUserAsync(adminId, userId);

                if (!success)
                    return NotFound(new { message = "User not found" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting user", error = ex.Message });
            }
        }

        [HttpGet("students")]
        [PermissionRequired("UserManagement", "read", "students")]
        public async Task<ActionResult<PaginationResponseDto<StudentProfileResponseDto>>> GetStudents(
            [FromQuery] PaginationRequestDto request,
            [FromQuery] string? academicStatus = null)
        {
            try
            {
                var adminId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                var result = await _userManagementService.GetStudentProfilesAsync(adminId, request, academicStatus);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching students", error = ex.Message });
            }
        }

        [HttpGet("students/{studentId}")]
        [PermissionRequired("UserManagement", "read", "student")]
        public async Task<ActionResult<StudentProfileResponseDto>> GetStudent(int studentId)
        {
            try
            {
                var adminId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                var student = await _userManagementService.GetStudentProfileByIdAsync(adminId, studentId);

                if (student == null)
                    return NotFound(new { message = "Student not found" });

                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching student", error = ex.Message });
            }
        }

        [HttpPut("students/{studentId}")]
        [PermissionRequired("UserManagement", "update", "student")]
        public async Task<ActionResult<StudentProfileResponseDto>> UpdateStudent(
            int studentId, [FromBody] StudentProfileUpdateDto updateDto)
        {
            try
            {
                var adminId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                var success = await _userManagementService.UpdateStudentProfileAsync(adminId, studentId, updateDto);

                if (!success)
                    return NotFound(new { message = "Student not found or no changes made" });

                var updatedStudent = await _userManagementService.GetStudentProfileByIdAsync(adminId, studentId);
                return Ok(updatedStudent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating student", error = ex.Message });
            }
        }

        [HttpGet("teachers")]
        [PermissionRequired("UserManagement", "read", "teachers")]
        public async Task<ActionResult<PaginationResponseDto<TeacherProfileResponseDto>>> GetTeachers(
            [FromQuery] PaginationRequestDto request,
            [FromQuery] int? depId = null)
        {
            try
            {
                var adminId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                var result = await _userManagementService.GetTeacherProfilesAsync(adminId, request, depId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching teachers", error = ex.Message });
            }
        }

        [HttpGet("teachers/{teacherId}")]
        [PermissionRequired("UserManagement", "read", "teacher")]
        public async Task<ActionResult<TeacherProfileResponseDto>> GetTeacher(int teacherId)
        {
            try
            {
                var adminId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                var teacher = await _userManagementService.GetTeacherProfileByIdAsync(adminId, teacherId);

                if (teacher == null)
                    return NotFound(new { message = "Teacher not found" });

                return Ok(teacher);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching teacher", error = ex.Message });
            }
        }

        [HttpPut("teachers/{teacherId}")]
        [PermissionRequired("UserManagement", "update", "teacher")]
        public async Task<ActionResult<TeacherProfileResponseDto>> UpdateTeacher(
     int teacherId, [FromBody] TeacherProfileUpdateDto updateDto)
        {
            try
            {
                var adminId = int.Parse(User.FindFirst("id")?.Value ?? "0");

                // Check if teacher exists first
                var existingTeacher = await _userManagementService.GetTeacherProfileByIdAsync(adminId, teacherId);
                if (existingTeacher == null)
                {
                    return NotFound(new { message = "Teacher not found" });
                }

                // Attempt update - this will return true even if no changes were made
                var success = await _userManagementService.UpdateTeacherProfileAsync(adminId, teacherId, updateDto);

                if (!success)
                {
                    // This should only happen if there was an unexpected error
                    return StatusCode(500, new { message = "An error occurred while updating teacher" });
                }

                // Return the updated teacher data
                var updatedTeacher = await _userManagementService.GetTeacherProfileByIdAsync(adminId, teacherId);
                return Ok(updatedTeacher);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating teacher", error = ex.Message });
            }
        }

        [HttpGet("admins")]
        [PermissionRequired("UserManagement", "read", "admins")]
        public async Task<ActionResult<PaginationResponseDto<AdminProfileResponseDto>>> GetAdmins(
            [FromQuery] PaginationRequestDto request,
            [FromQuery] string? level = null)
        {
            try
            {
                var adminId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                var result = await _userManagementService.GetAdminProfilesAsync(adminId, request, level);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching admins", error = ex.Message });
            }
        }

        [HttpGet("admins/{adminProfileId}")]
        [PermissionRequired("UserManagement", "read", "admin")]
        public async Task<ActionResult<AdminProfileResponseDto>> GetAdmin(int adminProfileId)
        {
            try
            {
                var adminId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                var admin = await _userManagementService.GetAdminProfileByIdAsync(adminId, adminProfileId);

                if (admin == null)
                    return NotFound(new { message = "Admin not found" });

                return Ok(admin);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching admin", error = ex.Message });
            }
        }

        [HttpPut("admins/{adminProfileId}")]
        [PermissionRequired("UserManagement", "update", "admin")]
        public async Task<ActionResult<AdminProfileResponseDto>> UpdateAdmin(
     int adminProfileId, [FromBody] AdminProfileUpdateDto updateDto)
        {
            try
            {
                var adminId = int.Parse(User.FindFirst("id")?.Value ?? "0");

                var existingAdmin = await _userManagementService.GetAdminProfileByIdAsync(adminId, adminProfileId);
                if (existingAdmin == null)
                {
                    return NotFound(new { message = "Admin not found" });
                }

                var success = await _userManagementService.UpdateAdminProfileAsync(adminId, adminProfileId, updateDto);

                if (!success)
                {
                    return StatusCode(500, new
                    {
                        message = "Failed to update admin profile",
                        details = "Please check the server logs for more information"
                    });
                }

                var updatedAdmin = await _userManagementService.GetAdminProfileByIdAsync(adminId, adminProfileId);
                return Ok(updatedAdmin);
            }
            catch (Exception ex)
            {
                // Log the full exception for debugging
                Console.WriteLine($"Error in UpdateAdmin: {ex}");

                return StatusCode(500, new
                {
                    message = "An error occurred while updating admin",
                    error = ex.Message,
                    details = ex.InnerException?.Message
                });
            }
        }
    }
}