// Controllers/CourseHistoryController.cs
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
    public class CourseHistoryController : ControllerBase
    {
        private readonly ICourseHistoryService _courseHistoryService;

        public CourseHistoryController(ICourseHistoryService courseHistoryService)
        {
            _courseHistoryService = courseHistoryService;
        }

        [HttpPost]
        [PermissionRequired("CourseHistory", "create", "course_history")]
        public async Task<IActionResult> AddCourseHistory([FromBody] CourseHistoryCreateDto dto)
        {
            try
            {
                var createdBy = int.Parse(User.FindFirstValue("id")!);
                var result = await _courseHistoryService.AddCourseHistoryAsync(dto, createdBy);
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [PermissionRequired("CourseHistory", "update", "course_history")]
        public async Task<IActionResult> UpdateCourseHistory(int id, [FromBody] CourseHistoryUpdateDto dto)
        {
            try
            {
                var modifiedBy = int.Parse(User.FindFirstValue("id")!);
                var result = await _courseHistoryService.UpdateCourseHistoryAsync(id, dto, modifiedBy);
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [PermissionRequired("CourseHistory", "delete", "course_history")]
        public async Task<IActionResult> DeleteCourseHistory(int id)
        {
            try
            {
                var modifiedBy = int.Parse(User.FindFirstValue("id")!);
                var result = await _courseHistoryService.DeleteCourseHistoryAsync(id, modifiedBy);
                return Ok(new { success = true, message = "Course history deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [PermissionRequired("CourseHistory", "read", "course_history")]
        public async Task<IActionResult> GetCourseHistoryById(int id)
        {
            try
            {
                var result = await _courseHistoryService.GetCourseHistoryByIdAsync(id);
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("student/{studentId}")]
        [PermissionRequired("CourseHistory", "read", "course_history")]
        public async Task<IActionResult> GetCourseHistoryByStudentId(int studentId)
        {
            try
            {
                var result = await _courseHistoryService.GetCourseHistoryByStudentIdAsync(studentId);
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [PermissionRequired("CourseHistory", "read", "course_history")]
        public async Task<IActionResult> GetCourseHistoryByFilters(
            [FromQuery] int? studentId,
            [FromQuery] int? courseId,
            [FromQuery] int? semesterId,
            [FromQuery] string? status,
            [FromQuery] bool? isRetake,
            [FromQuery] string? academicYear,
            [FromQuery] int? limit = 50,
            [FromQuery] int? offset = 0)
        {
            try
            {
                var result = await _courseHistoryService.GetCourseHistoryByFiltersAsync(
                    studentId, courseId, semesterId, status, isRetake, academicYear, limit, offset);

                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("calculate-gpa/{studentId}")]
        [PermissionRequired("CourseHistory", "update", "course_history")]
        public async Task<IActionResult> CalculateStudentGpa(int studentId)
        {
            try
            {
                var result = await _courseHistoryService.CalculateStudentGpaAsync(studentId);
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("academic-summary/{studentId}")]
        [PermissionRequired("CourseHistory", "read", "course_history")]
        public async Task<IActionResult> GetStudentAcademicSummary(int studentId)
        {
            try
            {
                var result = await _courseHistoryService.GetStudentAcademicSummaryAsync(studentId);
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("calculate-term-gpa/{studentId}/{semesterId}")]
        [PermissionRequired("CourseHistory", "update", "course_history")]
        public async Task<IActionResult> CalculateTermGpa(int studentId, int semesterId)
        {
            try
            {
                var result = await _courseHistoryService.CalculateTermGpaAsync(studentId, semesterId);
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("calculate-all-term-gpa/{semesterId}")]
        [PermissionRequired("CourseHistory", "update", "course_history")]
        public async Task<IActionResult> CalculateAllTermGpaForSemester(int semesterId)
        {
            try
            {
                var result = await _courseHistoryService.CalculateAllTermGpaForSemesterAsync(semesterId);
                return Ok(new { success = true, message = $"Processed term GPA for {result} students" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}