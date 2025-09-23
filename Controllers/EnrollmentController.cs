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
		private readonly IProfileService _profileService;
		

		public EnrollmentController(IEnrollmentService enrollmentService, ILogger<EnrollmentController> logger)
		{
			_enrollmentService = enrollmentService;
			
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
				
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPost("request")]
		[PermissionRequired("enrollment", "create", "enrollment_request")]
		public async Task<IActionResult> RequestCourseEnrollment([FromBody] EnrollmentRequestDto request)
		{
			try
			{
				var userId = GetCurrentUserId();
				var resolvedStudentId = await GetCurrentStudentId();
				if (resolvedStudentId == null) return Forbid();

				var dto = new EnrollmentRequestDto
				{
					StudentId = resolvedStudentId.Value,
					CourseOfferingId = request.CourseOfferingId
				};

				var result = await _enrollmentService.RequestCourseEnrollment(dto, userId);
				if (result.Id <= 0 && !string.IsNullOrEmpty(result.Message))
					return BadRequest(new { message = result.Message });
				return Ok(result);
			}
			catch (Exception ex)
			{
				 
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("available-courses")]
		[PermissionRequired("enrollment", "read", "available_courses")]
		public async Task<IActionResult> GetAvailableCourses()
		{
			try
			{
				var resolvedStudentId = await GetCurrentStudentId();
				if (resolvedStudentId == null) return Forbid();
				var courses = await _enrollmentService.GetAvailableCoursesForStudent(resolvedStudentId.Value);
				return Ok(courses);
			}
			catch (Exception ex)
			{
				
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("my-courses")]
		[PermissionRequired("enrollment", "read", "student_courses")]
		public async Task<IActionResult> GetMyCourses()
		{
			try
			{
				var resolvedStudentId = await GetCurrentStudentId();
				if (resolvedStudentId == null) return Forbid();
				var courses = await _enrollmentService.GetEnrolledCoursesForStudent(resolvedStudentId.Value);
				return Ok(courses);
			}
			catch (Exception ex)
			{
				
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpDelete("cancel/{enrollmentId}")]
		[PermissionRequired("enrollment", "delete", "cancel_enrollment")]
		public async Task<IActionResult> CancelEnrollmentRequest(int enrollmentId)
		{
			try
			{
				var userId = GetCurrentUserId();
				var resolvedStudentId = await GetCurrentStudentId();
				if (resolvedStudentId == null) return Forbid();
				var success = await _enrollmentService.CancelEnrollmentRequest(enrollmentId, resolvedStudentId.Value, userId);

				return success ? Ok(new { message = "Enrollment request canceled successfully" })
								  : BadRequest(new { message = "Failed to cancel enrollment request" });
			}
			catch (Exception ex)
			{
				
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("credit-status")]
		[PermissionRequired("enrollment", "read", "credit_status")]
		public async Task<IActionResult> GetCreditHourStatus()
		{
			try
			{
				var resolvedStudentId = await GetCurrentStudentId();
				if (resolvedStudentId == null) return Forbid();
				var status = await _enrollmentService.GetCreditHourStatus(resolvedStudentId.Value);
				return Ok(status);
			}
			catch (Exception ex)
			{
				
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPost("withdraw/{enrollmentId}")]
		[PermissionRequired("enrollment", "delete", "course_withdrawal")]
		public async Task<IActionResult> WithdrawFromCourse(int enrollmentId, [FromBody] WithdrawalRequestDto request)
		{
			try
			{
				var userId = GetCurrentUserId();
				var resolvedStudentId = await GetCurrentStudentId();
				if (resolvedStudentId == null) return Forbid();
				var success = await _enrollmentService.WithdrawFromCourse(
					enrollmentId, resolvedStudentId.Value, userId, request.Reason);

				return success ? Ok(new { message = "Course withdrawal successful" })
								  : BadRequest(new { message = "Failed to withdraw from course" });
			}
			catch (Exception ex)
			{
				
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("teacher/courses")]
		[PermissionRequired("enrollment", "read", "teacher_courses")]
		public async Task<IActionResult> GetTeacherCourses()
		{
			try
			{
				var teacherUserId = GetCurrentUserId();
				var service = HttpContext.RequestServices.GetService(typeof(backend.Repositories.IProfileRepository));
				var profileRepo = service as backend.Repositories.IProfileRepository;
				if (profileRepo == null) return Forbid();
				var teacherId = await profileRepo.GetTeacherIdByUserId(teacherUserId);
				if (teacherId == null) return Ok(Array.Empty<CourseOfferingDto>());
				var courses = await _enrollmentService.GetAssignedCoursesForTeacher(teacherId.Value);
				return Ok(courses);
			}
			catch (Exception ex)
			{
				
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("teacher/course/{courseOfferingId}/students")]
		[PermissionRequired("enrollment", "read", "course_students")]
		public async Task<IActionResult> GetCourseStudents(int courseOfferingId)
		{
			try
			{
				var teacherUserId = GetCurrentUserId();
				var service = HttpContext.RequestServices.GetService(typeof(backend.Repositories.IProfileRepository));
				var profileRepo = service as backend.Repositories.IProfileRepository;
				if (profileRepo == null) return Forbid();
				var teacherId = await profileRepo.GetTeacherIdByUserId(teacherUserId);
				if (teacherId == null) return Forbid();
				var students = await _enrollmentService.GetEnrolledStudentsForCourse(teacherId.Value, courseOfferingId);
				return Ok(students);
			}
			catch (Exception ex)
			{
				
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("teacher/course/{courseOfferingId}")]
		[PermissionRequired("enrollment", "read", "teacher_course_details")]
		public async Task<IActionResult> GetCourseDetailsForTeacher(int courseOfferingId)
		{
			try
			{
				var teacherUserId = GetCurrentUserId();
				var service = HttpContext.RequestServices.GetService(typeof(backend.Repositories.IProfileRepository));
				var profileRepo = service as backend.Repositories.IProfileRepository;
				if (profileRepo == null) return Forbid();
				var teacherId = await profileRepo.GetTeacherIdByUserId(teacherUserId);
				if (teacherId == null) return Forbid();
				var details = await _enrollmentService.GetCourseDetailsForTeacher(teacherId.Value, courseOfferingId);
				return Ok(details);
			}
			catch (Exception ex)
			{
				
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("teacher/student/{studentId}/history")]
		[PermissionRequired("enrollment", "read", "student_history")]
		public async Task<IActionResult> GetStudentAcademicHistory(int studentId)
		{
			try
			{
				var teacherUserId = GetCurrentUserId();
				var service = HttpContext.RequestServices.GetService(typeof(backend.Repositories.IProfileRepository));
				var profileRepo = service as backend.Repositories.IProfileRepository;
				if (profileRepo == null) return Forbid();
				var teacherId = await profileRepo.GetTeacherIdByUserId(teacherUserId);
				if (teacherId == null) return Forbid();
				var history = await _enrollmentService.GetStudentAcademicHistory(teacherId.Value, studentId);
				return Ok(history);
			}
			catch (Exception ex)
			{
				 
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("status-history/{enrollmentId}")]
		[PermissionRequired("enrollment", "read", "enrollment_status_history")]
		public async Task<IActionResult> GetEnrollmentStatusHistory(int enrollmentId)
		{
			try
			{
				var history = await _enrollmentService.GetEnrollmentStatusHistory(enrollmentId);
				return Ok(history);
			}
			catch (Exception ex)
			{
				
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("check-prerequisites")]
		[PermissionRequired("enrollment", "read", "check_prerequisites")]
		public async Task<IActionResult> CheckCoursePrerequisites([FromQuery] int courseId)
		{
			try
			{
				var resolvedStudentId = await GetCurrentStudentId();
				if (resolvedStudentId == null) return Forbid();
				var result = await _enrollmentService.CheckCoursePrerequisites(resolvedStudentId.Value, courseId);
				return Ok(result);
			}
			catch (Exception ex)
			{
				
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("current-course-load")]
		[PermissionRequired("enrollment", "read", "current_course_load")]
		public async Task<IActionResult> GetStudentCurrentCourseLoad()
		{
			try
			{
				var resolvedStudentId = await GetCurrentStudentId();
				if (resolvedStudentId == null) return Forbid();
				var load = await _enrollmentService.GetStudentCurrentCourseLoad(resolvedStudentId.Value);
				return Ok(load);
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

			if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
			{
				throw new UnauthorizedAccessException("Invalid user ID in token");
			}

			return userId;
		}

		private async Task<int?> GetCurrentStudentId()
		{
			var userId = GetCurrentUserId();
			var service = HttpContext.RequestServices.GetService(typeof(backend.Repositories.IProfileRepository));
			var profileRepo = service as backend.Repositories.IProfileRepository;
			if (profileRepo == null) return null;
			return await profileRepo.GetStudentIdByUserId(userId);
		}
	}
}