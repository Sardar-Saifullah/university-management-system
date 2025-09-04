// Controllers/SemesterController.cs
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
    public class SemesterController : ControllerBase
    {
        private readonly ISemesterService _service;

        public SemesterController(ISemesterService service)
        {
            _service = service;
        }

        [HttpPost]
        [PermissionRequired("Semester", "create", "semester")]
        public async Task<ActionResult<GetSemesterDto>> Create(CreateSemesterDto dto)
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var result = await _service.Create(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        [PermissionRequired("Semester", "read", "semester_single")]
        public async Task<ActionResult<GetSemesterDto>> GetById(int id)
        {
            var result = await _service.GetById(id);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet]
        [PermissionRequired("Semester", "read", "semester_list")]
        public async Task<ActionResult<IEnumerable<GetSemesterDto>>> GetAll()
        {
            var result = await _service.GetAll();
            return Ok(result);
        }

        [HttpGet("current")]
        [AllowAnonymous] // Allow anyone to check current semester
        public async Task<ActionResult<GetSemesterDto>> GetCurrent()
        {
            var result = await _service.GetCurrent();
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPut("{id}")]
        [PermissionRequired("Semester", "update", "semester")]
        public async Task<ActionResult<GetSemesterDto>> Update(int id, UpdateSemesterDto dto)
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var result = await _service.Update(id, dto, userId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [PermissionRequired("Semester", "delete", "semester")]
        public async Task<ActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var success = await _service.Delete(id, userId);
            return success ? NoContent() : BadRequest();
        }
    }
}