// Controllers/CourseOfferingController.cs
using backend.Dtos;
using backend.Filters;
using backend.Services;
using backend.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CourseOfferingController : ControllerBase
    {
        private readonly ICourseOfferingService _service;
        private readonly IWebHostEnvironment _environment;

        public CourseOfferingController(ICourseOfferingService service, IWebHostEnvironment environment)
        {
            _service = service;
            _environment = environment;
        }

        [HttpPost]
        [PermissionRequired("CourseOffering", "create", "course_offering")]
        public async Task<ActionResult<CourseOfferingGetDto>> Create(CourseOfferingCreateDto dto)
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            var result = await _service.Create(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [PermissionRequired("CourseOffering", "update", "course_offering")]
        public async Task<ActionResult<CourseOfferingGetDto>> Update(int id, CourseOfferingUpdateDto dto)
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            var result = await _service.Update(id, dto, userId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [PermissionRequired("CourseOffering", "delete", "course_offering")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            var success = await _service.Delete(id, userId);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpGet("{id}")]
        [PermissionRequired("CourseOffering", "read", "course_offering")]
        public async Task<ActionResult<CourseOfferingGetDto>> GetById(int id)
        {
            var result = await _service.GetById(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet]
        [PermissionRequired("CourseOffering", "read", "course_offering")]
        public async Task<ActionResult<IEnumerable<CourseOfferingGetDto>>> GetAll()
        {
            var results = await _service.GetAll();
            return Ok(results);
        }

        [HttpPost("bulk-upload")]
        [PermissionRequired("CourseOffering", "create", "course_offering")]
        public async Task<ActionResult<BulkUploadResultDto>> BulkUpload(IFormFile file)
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            var jsonProcessor = new JsonFileProcessor(_environment);
            var jsonDoc = await jsonProcessor.ProcessJsonFile(file);
            var result = await _service.BulkUpload(jsonDoc, userId);
            return Ok(result);
        }
    }
}