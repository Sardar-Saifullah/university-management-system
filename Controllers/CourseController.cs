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
            var course = await _courseService.GetCourse(id);
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
            [FromQuery] string searchTerm = null)
        {
            var courses = await _courseService.GetCourses(
                page, pageSize, departmentId, programId, levelId, isElective, searchTerm);
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

        [HttpGet("{courseId}/prerequisites")]
        [PermissionRequired("Courses", "read", "prerequisite")]
        public async Task<ActionResult<IEnumerable<PrerequisiteDto>>> GetCoursePrerequisites(int courseId)
        {
            var prerequisites = await _courseService.GetCoursePrerequisites(courseId);
            return Ok(prerequisites);
        }

        [HttpPost("prerequisites")]
        [PermissionRequired("Courses", "create", "prerequisite")]
        public async Task<ActionResult<PrerequisiteDto>> AddPrerequisite(PrerequisiteDto dto)
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            var prerequisite = await _courseService.AddPrerequisite(dto, userId);
            if (prerequisite == null) return BadRequest("Failed to add prerequisite");
            return CreatedAtAction(nameof(GetCoursePrerequisites), new { courseId = dto.CourseId }, prerequisite);
        }

        [HttpDelete("prerequisites/{prerequisiteId}")]
        [PermissionRequired("Courses", "delete", "prerequisite")]
        public async Task<IActionResult> RemovePrerequisite(int prerequisiteId)
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            var success = await _courseService.RemovePrerequisite(prerequisiteId, userId);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}