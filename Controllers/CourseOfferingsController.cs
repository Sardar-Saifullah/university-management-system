using backend.Dtos;
using backend.Filters;
using backend.Services;
using backend.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(PermissionFilter))]
    public class CourseOfferingController : ControllerBase
    {
        private readonly ICourseOfferingService _service;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CourseOfferingController(ICourseOfferingService service, IWebHostEnvironment webHostEnvironment)
        {
            _service = service;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        [PermissionRequired("CourseOffering", "create", "course_offering")]
        public async Task<ActionResult<ApiResponse<CourseOfferingGetDto>>> Create(CourseOfferingCreateDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("id").Value);
                var result = await _service.Create(dto, userId);

                return CreatedAtAction(nameof(GetById), new { id = result.Id },
                    new ApiResponse<CourseOfferingGetDto>
                    {
                        Success = true,
                        Data = result,
                        Message = "Course offering created successfully"
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        [PermissionRequired("CourseOffering", "update", "course_offering")]
        public async Task<ActionResult<ApiResponse<CourseOfferingGetDto>>> Update(int id, CourseOfferingUpdateDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("id").Value);
                var result = await _service.Update(id, dto, userId);

                if (result == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Course offering not found"
                    });
                }

                return Ok(new ApiResponse<CourseOfferingGetDto>
                {
                    Success = true,
                    Data = result,
                    Message = "Course offering updated successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        [PermissionRequired("CourseOffering", "delete", "course_offering")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("id").Value);
                var success = await _service.Delete(id, userId);

                if (!success)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Course offering not found or cannot be deleted"
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Course offering deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        [PermissionRequired("CourseOffering", "read", "course_offering")]
        public async Task<ActionResult<ApiResponse<CourseOfferingGetDto>>> GetById(int id)
        {
            try
            {
                var result = await _service.GetById(id);

                if (result == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Course offering not found"
                    });
                }

                return Ok(new ApiResponse<CourseOfferingGetDto>
                {
                    Success = true,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet]
        [PermissionRequired("CourseOffering", "read", "course_offering")]
        public async Task<ActionResult<ApiResponse<IEnumerable<CourseOfferingGetDto>>>> GetAll()
        {
            try
            {
                var results = await _service.GetAll();

                return Ok(new ApiResponse<IEnumerable<CourseOfferingGetDto>>
                {
                    Success = true,
                    Data = results
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("bulk-upload")]
        [PermissionRequired("CourseOffering", "create", "course_offering")]
        public async Task<ActionResult<ApiResponse<BulkUploadResultDto>>> BulkUpload(IFormFile file)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("id").Value);

                // Fix: Pass IWebHostEnvironment to JsonFileProcessor constructor
                var jsonProcessor = new JsonFileProcessor(_webHostEnvironment);
                var jsonDoc = await jsonProcessor.ProcessJsonFile(file);
                var result = await _service.BulkUpload(jsonDoc, userId);

                return Ok(new ApiResponse<BulkUploadResultDto>
                {
                    Success = true,
                    Data = result,
                    Message = result.ErrorCount == 0
                        ? "All course offerings uploaded successfully"
                        : $"{result.SuccessCount} offerings uploaded, {result.ErrorCount} failed"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }

  
}