using backend.Filters;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(SessionManagementFilter))]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionsController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPermissions()
        {
            var profile = User.FindFirst("profile").Value;
            var permissions = await _permissionService.GetProfilePermissions(profile);
            return Ok(permissions);
        }
    }
}