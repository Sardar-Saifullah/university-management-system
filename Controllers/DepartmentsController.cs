// Controllers/DepartmentsController.cs
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
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPost]
        [PermissionRequired("Department", "create", "department")]
        public async Task<ActionResult<DepartmentResponseDto>> CreateDepartment(DepartmentCreateDto departmentDto)
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            var department = await _departmentService.CreateDepartment(departmentDto, userId);
            return CreatedAtAction(nameof(GetDepartment), new { id = department.Id }, department);
        }

        [HttpGet("{id}")]
        [PermissionRequired("Department", "read", "department")]
        public async Task<ActionResult<DepartmentResponseDto>> GetDepartment(int id)
        {
            var department = await _departmentService.GetDepartment(id);
            if (department == null) return NotFound();
            return Ok(department);
        }

        [HttpGet]
        [PermissionRequired("Department", "read", "department")]
        public async Task<ActionResult<IEnumerable<DepartmentResponseDto>>> GetAllDepartments()
        {
            var departments = await _departmentService.GetAllDepartments();
            return Ok(departments);
        }

        [HttpPut("{id}")]
        [PermissionRequired("Department", "update", "department")]
        public async Task<IActionResult> UpdateDepartment(int id, DepartmentUpdateDto departmentDto)
        {
            if (id != departmentDto.Id) return BadRequest();
            var userId = int.Parse(User.FindFirst("id").Value);
            var department = await _departmentService.UpdateDepartment(id, departmentDto, userId);
            return Ok(department);
        }

        [HttpDelete("{id}")]
        [PermissionRequired("Department", "delete", "department")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            var result = await _departmentService.DeleteDepartment(id, userId);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}