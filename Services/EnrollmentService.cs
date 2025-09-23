// Services/Implementations/EnrollmentService.cs
using backend.Dtos;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
	public class EnrollmentService : IEnrollmentService
	{
		private readonly IEnrollmentRepository _enrollmentRepository;
		private readonly IProfileRepository _studentRepository;

		public EnrollmentService(IEnrollmentRepository enrollmentRepository, IProfileRepository studentRepository)
		{
			_enrollmentRepository = enrollmentRepository;
			_studentRepository = studentRepository;

		}

		public async Task<EnrollmentResponseDto> AdminEnrollStudent(EnrollmentRequestDto request, int adminId, int createdBy)
		{
			try
			{
				var enrollmentId = await _enrollmentRepository.AdminEnrollStudent(
					request.StudentId, request.CourseOfferingId, adminId, createdBy);

				if (enrollmentId <= 0)
				{
					throw new Exception("Failed to enroll student");
				}

				return new EnrollmentResponseDto
				{
					Id = enrollmentId,
					StudentId = request.StudentId,
					CourseOfferingId = request.CourseOfferingId,
					Status = "approved",
					EnrollmentDate = DateTime.UtcNow,
					ApprovalDate = DateTime.UtcNow,
					Message = "Student enrolled successfully"
				};
			}
			catch (Exception ex)
			{
				
				throw;
			}
		}

		public async Task<bool> AdminUnenrollStudent(int studentId, int courseOfferingId, int adminId, int modifiedBy)
		{
			try
			{
				var rowsAffected = await _enrollmentRepository.AdminUnenrollStudent(
					studentId, courseOfferingId, adminId, modifiedBy);

				return rowsAffected > 0;
			}
			catch (Exception ex)
			{
				
				throw;
			}
		}

		public async Task<bool> ApproveEnrollment(int enrollmentId, int adminId, int modifiedBy)
		{
			try
			{
				var rowsAffected = await _enrollmentRepository.ApproveEnrollment(
					enrollmentId, adminId, modifiedBy);

				return rowsAffected > 0;
			}
			catch (Exception ex)
			{
				
				throw;
			}
		}

		public async Task<bool> RejectEnrollment(int enrollmentId, int adminId, string rejectionReason, int modifiedBy)
		{
			try
			{
				var rowsAffected = await _enrollmentRepository.RejectEnrollment(
					enrollmentId, adminId, rejectionReason, modifiedBy);

				return rowsAffected > 0;
			}
			catch (Exception ex)
			{
				
				throw;
			}
		}

		public async Task<IEnumerable<EnrollmentResponseDto>> GetPendingEnrollments()
		{
			try
			{
				var enrollments = await _enrollmentRepository.GetPendingEnrollments();
				return enrollments.Select(e => new EnrollmentResponseDto
				{
					Id = e.Id,
					StudentId = e.StudentId,
					CourseOfferingId = e.CourseOfferingId,
					Status = e.Status,
					EnrollmentDate = e.EnrollmentDate
				});
			}
			catch (Exception ex)
			{
				
				throw;
			}
		}

		public async Task<EnrollmentResponseDto> RequestCourseEnrollment(EnrollmentRequestDto request, int createdBy)
		{
			try
			{
				return await _enrollmentRepository.RequestCourseEnrollment(
					request.StudentId, request.CourseOfferingId, createdBy);
			}
			catch (Exception ex)
			{
				
				throw;
			}
		}

		public async Task<IEnumerable<CourseOfferingDto>> GetAvailableCoursesForStudent(int studentId)
		{
			try
			{
				return await _enrollmentRepository.GetAvailableCoursesForStudent(studentId);
			}
			catch (Exception ex)
			{
				
				throw;
			}
		}

		public async Task<IEnumerable<EnrolledCourseDto>> GetEnrolledCoursesForStudent(int studentId)
		{
			try
			{
				return await _enrollmentRepository.GetEnrolledCoursesForStudent(studentId);
			}
			catch (Exception ex)
			{
				
				throw;
			}
		}

		public async Task<bool> CancelEnrollmentRequest(int enrollmentId, int studentId, int modifiedBy)
		{
			try
			{
				var rowsAffected = await _enrollmentRepository.CancelEnrollmentRequest(
					enrollmentId, studentId, modifiedBy);

				return rowsAffected > 0;
			}
			catch (Exception ex)
			{
				
				throw;
			}
		}

		public async Task<IEnumerable<EnrolledCourseDto>> GetEnrollmentHistoryForStudent(int studentId)
		{
			try
			{
				var enrollments = await _enrollmentRepository.GetEnrollmentHistoryForStudent(studentId);

				return enrollments.Select(e => new EnrolledCourseDto
				{
					EnrollmentId = e.Id,
					Status = e.Status,
					EnrollmentDate = e.EnrollmentDate,
					ApprovalDate = e.ApprovalDate,
					RejectionReason = e.RejectionReason,
					WithdrawalDate = e.WithdrawalDate,
					DropDate = e.DropDate
				});
			}
			catch (Exception ex)
			{
				
				throw;
			}
		}

		public async Task<CreditHourStatusDto> GetCreditHourStatus(int studentId)
		{
			try
			{
				var status = await _enrollmentRepository.GetCreditHourStatus(studentId);
				if (status == null) throw new Exception("Credit status not found");
				return status;
			}
			catch (Exception ex)
			{
				
				throw;
			}
		}

		public async Task<bool> WithdrawFromCourse(int enrollmentId, int studentId, int modifiedBy, string reason)
		{
			try
			{
				var rowsAffected = await _enrollmentRepository.WithdrawFromCourse(
					enrollmentId, studentId, modifiedBy, reason);

				return rowsAffected > 0;
			}
			catch (Exception ex)
			{
				
				throw;
			}
		}

		public async Task<IEnumerable<CourseOfferingDto>> GetAssignedCoursesForTeacher(int teacherId)
		{
			try
			{
				return await _enrollmentRepository.GetAssignedCoursesForTeacher(teacherId);
			}
			catch (Exception ex)
			{
				
				throw;
			}
		}

		public async Task<IEnumerable<EnrolledStudentDto>> GetEnrolledStudentsForCourse(int teacherId, int courseOfferingId)
		{
			try
			{
				return await _enrollmentRepository.GetEnrolledStudentsForCourse(teacherId, courseOfferingId);
			}
			catch (Exception ex)
			{
				
				throw;
			}
		}

		public async Task<CourseOfferingDto> GetCourseDetailsForTeacher(int teacherId, int courseOfferingId)
		{
			try
			{
				var dto = await _enrollmentRepository.GetCourseDetailsForTeacher(teacherId, courseOfferingId);
				if (dto == null)
				{
					throw new Exception("Course offering not found");
				}
				return dto;
			}
			catch (Exception ex)
			{
				
				throw;
			}
		}

		public async Task<StudentProfile> GetStudentAcademicHistory(int teacherId, int studentId)
		{
			try
			{
				return await _enrollmentRepository.GetStudentAcademicHistory(teacherId, studentId);
			}
			catch (Exception ex)
			{
				
				throw;
			}
		}

		public async Task<IEnumerable<EnrollmentStatusHistoryDto>> GetEnrollmentStatusHistory(int enrollmentId)
		{
			try
			{
				return await _enrollmentRepository.GetEnrollmentStatusHistory(enrollmentId);
			}
			catch (Exception ex)
			{
				
				throw;
			}
		}

		public async Task<int> OverrideCreditLimit(CreditLimitOverrideDto request, int approvedBy)
		{
			try
			{
				return await _enrollmentRepository.OverrideCreditLimit(
					request.StudentId, request.PolicyId, request.NewMaxCredits,
					request.Reason, approvedBy, request.ExpiresAt);
			}
			catch (Exception ex)
			{
				
				throw;
			}
		}

		public async Task<PrerequisiteCheckDto> CheckCoursePrerequisites(int studentId, int courseId)
		{
			try
			{
				return await _enrollmentRepository.CheckCoursePrerequisites(studentId, courseId);
			}
			catch (Exception ex)
			{
				
				throw;
			}
		}

		public async Task<CourseLoadDto> GetStudentCurrentCourseLoad(int studentId)
		{
			try
			{
				return await _enrollmentRepository.GetStudentCurrentCourseLoad(studentId);
			}
			catch (Exception ex)
			{
				
				throw;
			}
		}
	}
}