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
    public class AcademicProgramController : ControllerBase
    {
        private readonly IAcademicProgramService _programService;

        public AcademicProgramController(IAcademicProgramService programService)
        {
            _programService = programService;
        }

        [HttpPost]
        [PermissionRequired("AcademicProgram", "create", "program")]
        public async Task<ActionResult<ProgramResponseDto>> CreateProgram(ProgramCreateDto dto)
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            var program = await _programService.CreateProgram(dto, userId);
            return CreatedAtAction(nameof(GetProgram), new { id = program.Id }, program);
        }

        [HttpGet("{id}")]
        [PermissionRequired("AcademicProgram", "read", "program")]
        public async Task<ActionResult<ProgramResponseDto>> GetProgram(int id)
        {
            var program = await _programService.GetProgram(id);
            if (program == null) return NotFound();
            return Ok(program);
        }

        [HttpGet]
        [PermissionRequired("AcademicProgram", "read", "program")]
        public async Task<ActionResult<IEnumerable<ProgramResponseDto>>> GetAllPrograms()
        {
            var programs = await _programService.GetAllPrograms();
            return Ok(programs);
        }

        [HttpPut("{id}")]
        [PermissionRequired("AcademicProgram", "update", "program")]
        public async Task<ActionResult<ProgramResponseDto>> UpdateProgram(int id, ProgramUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");

            var userId = int.Parse(User.FindFirst("id").Value);
            var program = await _programService.UpdateProgram(dto, userId);

            if (program == null) return NotFound();
            return Ok(program);
        }

        [HttpDelete("{id}")]
        [PermissionRequired("AcademicProgram", "delete", "program")]
        public async Task<IActionResult> DeleteProgram(int id)
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            var success = await _programService.DeleteProgram(id, userId);

            if (!success) return NotFound();
            return NoContent();
        }
    }
}