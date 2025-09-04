// Controllers/CreditLimitOverrideController.cs
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
    public class CreditLimitOverrideController : ControllerBase
    {
        private readonly ICreditLimitOverrideService _overrideService;

        public CreditLimitOverrideController(ICreditLimitOverrideService overrideService)
        {
            _overrideService = overrideService;
        }

        [HttpPost]
        [PermissionRequired("CreditLimitOverride", "create", "credit_limit_override")]
        public async Task<IActionResult> Create([FromBody] CreditLimitOverrideCreateDto dto)
        {
            try
            {
                var approvedBy = GetCurrentUserId();
                var result = await _overrideService.CreateAsync(dto, approvedBy);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [PermissionRequired("CreditLimitOverride", "update", "credit_limit_override")]
        public async Task<IActionResult> Update(int id, [FromBody] CreditLimitOverrideUpdateDto dto)
        {
            try
            {
                var modifiedBy = GetCurrentUserId();
                var result = await _overrideService.UpdateAsync(id, dto, modifiedBy);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("student/{studentId}")]
        [PermissionRequired("CreditLimitOverride", "read", "credit_limit_override")]
        public async Task<IActionResult> GetByStudentId(int studentId)
        {
            try
            {
                var result = await _overrideService.GetByStudentIdAsync(studentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [PermissionRequired("CreditLimitOverride", "read", "credit_limit_override")]
        public async Task<IActionResult> GetAll([FromQuery] bool? activeOnly, [FromQuery] int? departmentId, [FromQuery] int? programId)
        {
            try
            {
                var result = await _overrideService.GetAllAsync(activeOnly, departmentId, programId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [PermissionRequired("CreditLimitOverride", "read", "credit_limit_override")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _overrideService.GetByIdAsync(id);
                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [PermissionRequired("CreditLimitOverride", "delete", "credit_limit_override")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var modifiedBy = GetCurrentUserId();
                var success = await _overrideService.DeleteAsync(id, modifiedBy);
                return Ok(new { success });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                            ?? User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("Invalid user ID in token");
            }

            return userId;
        }
    }
}