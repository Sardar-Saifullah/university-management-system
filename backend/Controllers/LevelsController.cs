// Controllers/LevelsController.cs
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
    public class LevelsController : ControllerBase
    {
        private readonly ILevelService _service;

        public LevelsController(ILevelService service)
        {
            _service = service;
        }

        [HttpPost]
        [PermissionRequired("Level", "create", "level")]
        public async Task<ActionResult<LevelGetDto>> Create(LevelCreateDto dto)
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var level = await _service.Create(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = level.Id }, level);
        }

        [HttpGet("{id}")]
        [PermissionRequired("Level", "read", "level")]
        public async Task<ActionResult<LevelGetDto>> GetById(int id)
        {
            var level = await _service.GetById(id);
            return level == null ? NotFound() : Ok(level);
        }

        [HttpGet]
        [PermissionRequired("Level", "read", "level")]
        public async Task<ActionResult<IEnumerable<LevelGetDto>>> GetAll()
        {
            var levels = await _service.GetAll();
            return Ok(levels);
        }

        [HttpPut("{id}")]
        [PermissionRequired("Level", "update", "level")]
        public async Task<IActionResult> Update(int id, LevelUpdateDto dto)
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var level = await _service.Update(id, dto, userId);
            return level == null ? NotFound() : NoContent();
        }

        [HttpDelete("{id}")]
        [PermissionRequired("Level", "delete", "level")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var success = await _service.Delete(id, userId);
            return success ? NoContent() : NotFound();
        }
    }
}