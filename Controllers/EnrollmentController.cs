// Controllers/EnrollmentController.cs
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
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly ILogger<EnrollmentController> _logger;

        public EnrollmentController(IEnrollmentService enrollmentService, ILogger<EnrollmentController> logger)
        {
            _enrollmentService = enrollmentService;
            _logger = logger;
        }

        [HttpPost("admin/enroll")]
        [PermissionRequired("enrollment", "create", "admin_enrollment")]
        public async Task<IActionResult> AdminEnrollStudent([FromBody] EnrollmentRequestDto request)
        {
            try
            {
                var adminId = GetCurrentUserId();
                var result = await _enrollmentService.AdminEnrollStudent(request, adminId, adminId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AdminEnrollStudent");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("admin/unenroll")]
        [PermissionRequired("enrollment", "delete", "admin_enrollment")]
        public async Task<IActionResult> AdminUnenrollStudent([FromBody] EnrollmentRequestDto request)
        {
            try
            {
                var adminId = GetCurrentUserId();
                var success = await _enrollmentService.AdminUnenrollStudent(
                    request.StudentId, request.CourseOfferingId, adminId, adminId);

                return success ? Ok(new { message = "Student unenrolled successfully" })
                              : BadRequest(new { message = "Failed to unenroll student" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AdminUnenrollStudent");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("approve/{enrollmentId}")]
        [PermissionRequired("enrollment", "update", "approve_enrollment")]
        public async Task<IActionResult> ApproveEnrollment(int enrollmentId)
        {
            try
            {
                var adminId = GetCurrentUserId();
                var success = await _enrollmentService.ApproveEnrollment(enrollmentId, adminId, adminId);

                return success ? Ok(new { message = "Enrollment approved successfully" })
                              : BadRequest(new { message = "Failed to approve enrollment" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ApproveEnrollment");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("reject/{enrollmentId}")]
        [PermissionRequired("enrollment", "update", "reject_enrollment")]
        public async Task<IActionResult> RejectEnrollment(int enrollmentId, [FromBody] EnrollmentApprovalDto approvalDto)
        {
            try
            {
                var adminId = GetCurrentUserId();
                var success = await _enrollmentService.RejectEnrollment(
                    enrollmentId, adminId, approvalDto.RejectionReason, adminId);

                return success ? Ok(new { message = "Enrollment rejected successfully" })
                              : BadRequest(new { message = "Failed to reject enrollment" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RejectEnrollment");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("pending")]
        [PermissionRequired("enrollment", "read", "pending_enrollments")]
        public async Task<IActionResult> GetPendingEnrollments()
        {
            try
            {
                var enrollments = await _enrollmentService.GetPendingEnrollments();
                return Ok(enrollments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetPendingEnrollments");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("request")]
        [PermissionRequired("enrollment", "create", "enrollment_request")]
        public async Task<IActionResult> RequestCourseEnrollment([FromBody] EnrollmentRequestDto request)
        {
            try
            {
                var studentId = GetCurrentUserId();
                if (request.StudentId != studentId)
                {
                    return Forbid("You can only request enrollment for yourself");
                }

                var result = await _enrollmentService.RequestCourseEnrollment(request, studentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RequestCourseEnrollment");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("available-courses")]
        [PermissionRequired("enrollment", "read", "available_courses")]
        public async Task<IActionResult> GetAvailableCourses()
        {
            try
            {
                var studentId = GetCurrentUserId();
                var courses = await _enrollmentService.GetAvailableCoursesForStudent(studentId);
                return Ok(courses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAvailableCourses");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("my-courses")]
        [PermissionRequired("enrollment", "read", "student_courses")]
        public async Task<IActionResult> GetMyCourses()
        {
            try
            {
                var studentId = GetCurrentUserId();
                var courses = await _enrollmentService.GetEnrolledCoursesForStudent(studentId);
                return Ok(courses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetMyCourses");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("cancel/{enrollmentId}")]
        [PermissionRequired("enrollment", "delete", "cancel_enrollment")]
        public async Task<IActionResult> CancelEnrollmentRequest(int enrollmentId)
        {
            try
            {
                var studentId = GetCurrentUserId();
                var success = await _enrollmentService.CancelEnrollmentRequest(enrollmentId, studentId, studentId);

                return success ? Ok(new { message = "Enrollment request canceled successfully" })
                              : BadRequest(new { message = "Failed to cancel enrollment request" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CancelEnrollmentRequest");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("credit-status")]
        [PermissionRequired("enrollment", "read", "credit_status")]
        public async Task<IActionResult> GetCreditHourStatus()
        {
            try
            {
                var studentId = GetCurrentUserId();
                var status = await _enrollmentService.GetCreditHourStatus(studentId);
                return Ok(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetCreditHourStatus");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("withdraw/{enrollmentId}")]
        [PermissionRequired("enrollment", "delete", "course_withdrawal")]
        public async Task<IActionResult> WithdrawFromCourse(int enrollmentId, [FromBody] WithdrawalRequestDto request)
        {
            try
            {
                var studentId = GetCurrentUserId();
                var success = await _enrollmentService.WithdrawFromCourse(
                    enrollmentId, studentId, studentId, request.Reason);

                return success ? Ok(new { message = "Course withdrawal successful" })
                              : BadRequest(new { message = "Failed to withdraw from course" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in WithdrawFromCourse");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("teacher/courses")]
        [PermissionRequired("enrollment", "read", "teacher_courses")]
        public async Task<IActionResult> GetTeacherCourses()
        {
            try
            {
                var teacherId = GetCurrentUserId();
                var courses = await _enrollmentService.GetAssignedCoursesForTeacher(teacherId);
                return Ok(courses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTeacherCourses");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("teacher/course/{courseOfferingId}/students")]
        [PermissionRequired("enrollment", "read", "course_students")]
        public async Task<IActionResult> GetCourseStudents(int courseOfferingId)
        {
            try
            {
                var teacherId = GetCurrentUserId();
                var students = await _enrollmentService.GetEnrolledStudentsForCourse(teacherId, courseOfferingId);
                return Ok(students);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetCourseStudents");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("teacher/student/{studentId}/history")]
        [PermissionRequired("enrollment", "read", "student_history")]
        public async Task<IActionResult> GetStudentAcademicHistory(int studentId)
        {
            try
            {
                var teacherId = GetCurrentUserId();
                var history = await _enrollmentService.GetStudentAcademicHistory(teacherId, studentId);
                return Ok(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetStudentAcademicHistory");
                return BadRequest(new { message = ex.Message });
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("Invalid user ID in token");
            }

            return userId;
        }
    }
}