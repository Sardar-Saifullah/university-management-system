using backend.Dtos;
using backend.Filters;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [ServiceFilter(typeof(PermissionFilter))]
    public class TeacherAssignmentController : ControllerBase
    {
        private readonly ITeacherAssignmentService _assignmentService;

        public TeacherAssignmentController(ITeacherAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        [HttpPost]
        [PermissionRequired("TeacherAssignment", "create", "teacher_assignment")]
        public async Task<ActionResult<ApiResponse<TeacherAssignmentResponseDto>>> AssignTeacherToCourse(
            [FromBody] TeacherAssignmentCreateDto dto)
        {
            try
            {
                var adminId = GetCurrentUserId();
                var result = await _assignmentService.AssignTeacherToCourseAsync(dto, adminId);
                return Ok(ApiResponse<TeacherAssignmentResponseDto>.SuccessResponse(result, "Teacher assigned successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TeacherAssignmentResponseDto>.ErrorResponse(ex.Message));
            }
        }

        [HttpPut("{id}")]
        [PermissionRequired("TeacherAssignment", "update", "teacher_assignment")]
        public async Task<ActionResult<ApiResponse<TeacherAssignmentResponseDto>>> UpdateAssignment(
            int id, [FromBody] TeacherAssignmentUpdateDto dto)
        {
            try
            {
                var adminId = GetCurrentUserId();
                var result = await _assignmentService.UpdateTeacherAssignmentAsync(id, dto, adminId);
                return Ok(ApiResponse<TeacherAssignmentResponseDto>.SuccessResponse(result, "Assignment updated successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TeacherAssignmentResponseDto>.ErrorResponse(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [PermissionRequired("TeacherAssignment", "delete", "teacher_assignment")]
        public async Task<ActionResult<ApiResponse<bool>>> RemoveAssignment(int id)
        {
            try
            {
                var adminId = GetCurrentUserId();
                var result = await _assignmentService.RemoveTeacherAssignmentAsync(id, adminId);
                return Ok(ApiResponse<bool>.SuccessResponse(result, "Assignment removed successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet]
        [PermissionRequired("TeacherAssignment", "read", "teacher_assignment")]
        public async Task<ActionResult<ApiResponse<IEnumerable<TeacherAssignmentResponseDto>>>> GetAllAssignments(
            [FromQuery] bool? activeOnly = true)
        {
            try
            {
                var adminId = GetCurrentUserId();
                var result = await _assignmentService.GetAllAssignmentsAsync(adminId, activeOnly);
                return Ok(ApiResponse<IEnumerable<TeacherAssignmentResponseDto>>.SuccessResponse(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<IEnumerable<TeacherAssignmentResponseDto>>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("my-assignments")]
        [PermissionRequired("TeacherAssignment", "read", "teacher_assignment_own")]
        public async Task<ActionResult<ApiResponse<IEnumerable<TeacherAssignmentResponseDto>>>> GetMyAssignments(
            [FromQuery] bool includeInactive = false)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _assignmentService.GetTeacherAssignmentsAsync(userId, includeInactive);
                return Ok(ApiResponse<IEnumerable<TeacherAssignmentResponseDto>>.SuccessResponse(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<IEnumerable<TeacherAssignmentResponseDto>>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("{id}")]
        [PermissionRequired("TeacherAssignment", "read", "teacher_assignment")]
        public async Task<ActionResult<ApiResponse<TeacherAssignmentResponseDto>>> GetAssignmentById(int id)
        {
            try
            {
                var result = await _assignmentService.GetAssignmentByIdAsync(id);
                return Ok(ApiResponse<TeacherAssignmentResponseDto>.SuccessResponse(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TeacherAssignmentResponseDto>.ErrorResponse(ex.Message));
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedAccessException("Invalid user ID in token");

            return userId;
        }
    }
}