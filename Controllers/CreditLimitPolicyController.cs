// Controllers/CreditLimitPolicyController.cs
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
    public class CreditLimitPolicyController : ControllerBase
    {
        private readonly ICreditLimitPolicyService _policyService;

        public CreditLimitPolicyController(ICreditLimitPolicyService policyService)
        {
            _policyService = policyService;
        }

        [HttpPost]
        [PermissionRequired("CreditLimitPolicy", "create", "credit_limit_policy")]
        public async Task<IActionResult> Create([FromBody] CreditLimitPolicyCreateDto dto)
        {
            try
            {
                var createdBy = GetCurrentUserId();
                var result = await _policyService.CreateAsync(dto, createdBy);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [PermissionRequired("CreditLimitPolicy", "update", "credit_limit_policy")]
        public async Task<IActionResult> Update(int id, [FromBody] CreditLimitPolicyUpdateDto dto)
        {
            try
            {
                var modifiedBy = GetCurrentUserId();
                var result = await _policyService.UpdateAsync(id, dto, modifiedBy);
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

        [HttpGet]
        [PermissionRequired("CreditLimitPolicy", "read", "credit_limit_policy")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _policyService.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [PermissionRequired("CreditLimitPolicy", "read", "credit_limit_policy")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _policyService.GetByIdAsync(id);
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
        [PermissionRequired("CreditLimitPolicy", "delete", "credit_limit_policy")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var modifiedBy = GetCurrentUserId();
                var success = await _policyService.DeleteAsync(id, modifiedBy);
                return Ok(new { success });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("student/{studentId}/effective-limit")]
        [PermissionRequired("CreditLimitPolicy", "read", "credit_limit")]
        public async Task<IActionResult> GetEffectiveCreditLimit(int studentId)
        {
            try
            {
                var result = await _policyService.GetEffectiveCreditLimitAsync(studentId);
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