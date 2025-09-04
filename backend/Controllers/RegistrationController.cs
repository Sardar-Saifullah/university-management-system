using backend.Dtos;
using backend.Filters;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(SessionManagementFilter))]
  
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpPost("student")]
        public async Task<IActionResult> RegisterStudent(StudentRegistrationDto request)
        {
            var createdBy = int.Parse(User.FindFirst("id").Value);
            var response = await _registrationService.RegisterStudent(request, createdBy);
            return response != null ? Ok(response) : BadRequest("Registration failed");
        }

        [HttpPost("teacher")]
        public async Task<IActionResult> RegisterTeacher(TeacherRegistrationDto request)
        {
            var createdBy = int.Parse(User.FindFirst("id").Value);
            var response = await _registrationService.RegisterTeacher(request, createdBy);
            return response != null ? Ok(response) : BadRequest("Registration failed");
        }

        [HttpPost("admin")]
        public async Task<IActionResult> RegisterAdmin(AdminRegistrationDto request)
        {
            var createdBy = int.Parse(User.FindFirst("id").Value);
            var response = await _registrationService.RegisterAdmin(request, createdBy);
            return response != null ? Ok(response) : BadRequest("Registration failed");
        }
    }
}