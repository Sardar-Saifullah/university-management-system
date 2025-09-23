using backend.Dtos;
using backend.Filters;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilePictureController : ControllerBase
    {
        private readonly IProfilePictureService _profilePictureService;

        public ProfilePictureController(IProfilePictureService profilePictureService)
        {
            _profilePictureService = profilePictureService;
        }

        [HttpPost]
        [PermissionRequired("ProfilePicture", "create", "profile_picture")]
        public async Task<IActionResult> CreateProfilePicture([FromForm] ProfilePictureUploadDto dto)
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var result = await _profilePictureService.CreateProfilePictureAsync(userId, dto);
            return result.Success
                ? Ok(new { result.PictureId, result.Message })
                : BadRequest(new { result.Message });
        }

        [HttpGet("{userId}")]
        [PermissionRequired("ProfilePicture", "read", "profile_picture")]
        public async Task<IActionResult> GetProfilePicture(int userId)
        {
            var requestingUserId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var result = await _profilePictureService.GetProfilePictureAsync(requestingUserId, userId);

            return result.Success
                ? Ok(result.Data)
                : StatusCode(result.Message.Contains("Unauthorized") ? 403 : 404, new { result.Message });
        }

        [HttpGet("my-picture")]
        [PermissionRequired("ProfilePicture", "read", "profile_picture")]
        public async Task<IActionResult> GetMyProfilePicture()
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var result = await _profilePictureService.GetMyProfilePictureAsync(userId);

            return result.Success
                ? Ok(result.Data)
                : StatusCode(404, new { result.Message });
        }

        [HttpDelete("{pictureId}")]
        [PermissionRequired("ProfilePicture", "delete", "profile_picture")]
        public async Task<IActionResult> DeleteProfilePicture(int pictureId)
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var result = await _profilePictureService.DeleteProfilePictureAsync(userId, pictureId);

            return result.Success
                ? Ok(new { result.Message })
                : BadRequest(new { result.Message });
        }
    }
}