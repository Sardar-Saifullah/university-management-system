using backend.Dtos;
using backend.Filters;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionsController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet("{profileName}")]
        [PermissionRequired("View Profile Permissions")]
        public async Task<ActionResult<IEnumerable<PermissionResponse>>> GetProfilePermissions(string profileName)
        {
            var result = await _permissionService.GetProfilePermissions(profileName);
            return Ok(result);
        }
    }
}