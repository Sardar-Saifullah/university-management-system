using backend.Dtos;
using backend.Filters;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpPost]
        [PermissionRequired("Courses", "create", "course")]
        public async Task<ActionResult<CourseResponseDto>> CreateCourse(CourseCreateDto dto)
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            var course = await _courseService.CreateCourse(dto, userId);
            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
        }

        [HttpGet("{id}")]
        [PermissionRequired("Courses", "read", "course")]
        public async Task<ActionResult<CourseResponseDto>> GetCourse(int id)
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            var course = await _courseService.GetCourse(id, userId);
            if (course == null) return NotFound();
            return Ok(course);
        }

        [HttpGet]
        [PermissionRequired("Courses", "read", "course")]
        public async Task<ActionResult<CourseListResponseDto>> GetCourses(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? departmentId = null,
            [FromQuery] int? programId = null,
            [FromQuery] int? levelId = null,
            [FromQuery] bool? isElective = null,
            [FromQuery] string searchTerm = null,
            [FromQuery] bool onlyActive = true)
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            var courses = await _courseService.GetCourses(
                userId, page, pageSize, departmentId, programId, levelId,
                isElective, searchTerm, onlyActive);
            return Ok(courses);
        }

        [HttpPut("{id}")]
        [PermissionRequired("Courses", "update", "course")]
        public async Task<ActionResult<CourseResponseDto>> UpdateCourse(int id, CourseUpdateDto dto)
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            var course = await _courseService.UpdateCourse(id, dto, userId);
            if (course == null) return NotFound();
            return Ok(course);
        }

        [HttpDelete("{id}")]
        [PermissionRequired("Courses", "delete", "course")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            var success = await _courseService.DeleteCourse(id, userId);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPost("bulk-upload")]
        [PermissionRequired("Courses", "create", "course")]
        public async Task<ActionResult<BulkUploadResultDto>> BulkUploadCourses(IFormFile file)
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            var result = await _courseService.BulkUploadCourses(file, userId);
            return Ok(result);
        }

        [HttpGet("search")]
        [PermissionRequired("Courses", "read", "course")]
        public async Task<ActionResult<CourseListResponseDto>> SearchCourses(
            [FromQuery] string searchTerm = null,
            [FromQuery] int? departmentId = null,
            [FromQuery] int? levelId = null,
            [FromQuery] bool? isElective = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            var courses = await _courseService.SearchCoursesLightweight(
                userId, searchTerm, departmentId, levelId, isElective, page, pageSize);
            return Ok(courses);
        }
    }
}