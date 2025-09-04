// Services/Implementations/EnrollmentService.cs
using backend.Dtos;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly ILogger<EnrollmentService> _logger;

        public EnrollmentService(IEnrollmentRepository enrollmentRepository, ILogger<EnrollmentService> logger)
        {
            _enrollmentRepository = enrollmentRepository;
            _logger = logger;
        }

        public async Task<EnrollmentResponseDto> AdminEnrollStudent(EnrollmentRequestDto request, int adminId, int createdBy)
        {
            try
            {
                var enrollmentId = await _enrollmentRepository.AdminEnrollStudent(
                    request.StudentId, request.CourseOfferingId, adminId, createdBy);

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
                _logger.LogError(ex, "Error in AdminEnrollStudent");
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
                _logger.LogError(ex, "Error in AdminUnenrollStudent");
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
                _logger.LogError(ex, "Error in ApproveEnrollment");
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
                _logger.LogError(ex, "Error in RejectEnrollment");
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
                _logger.LogError(ex, "Error in GetPendingEnrollments");
                throw;
            }
        }

        public async Task<EnrollmentResponseDto> RequestCourseEnrollment(EnrollmentRequestDto request, int createdBy)
        {
            try
            {
                var enrollmentId = await _enrollmentRepository.RequestCourseEnrollment(
                    request.StudentId, request.CourseOfferingId, createdBy);

                return new EnrollmentResponseDto
                {
                    Id = enrollmentId,
                    StudentId = request.StudentId,
                    CourseOfferingId = request.CourseOfferingId,
                    Status = "pending",
                    EnrollmentDate = DateTime.UtcNow,
                    Message = "Enrollment request submitted successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RequestCourseEnrollment");
                throw;
            }
        }

        public async Task<IEnumerable<CourseOfferingDto>> GetAvailableCoursesForStudent(int studentId)
        {
            try
            {
                var offerings = await _enrollmentRepository.GetAvailableCoursesForStudent(studentId);

                // Map to DTO - this would need additional data from related tables
                // For now, return basic information
                return offerings.Select(o => new CourseOfferingDto
                {
                    Id = o.Id,
                    // You would need to join with course, semester, department, etc. tables
                    // to get the full information
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAvailableCoursesForStudent");
                throw;
            }
        }

        public async Task<IEnumerable<EnrolledCourseDto>> GetEnrolledCoursesForStudent(int studentId)
        {
            try
            {
                var enrollments = await _enrollmentRepository.GetEnrolledCoursesForStudent(studentId);

                return enrollments.Select(e => new EnrolledCourseDto
                {
                    EnrollmentId = e.Id,
                    Status = e.Status,
                    EnrollmentDate = e.EnrollmentDate,
                    ApprovalDate = e.ApprovalDate,
                    RejectionReason = e.RejectionReason,
                    WithdrawalDate = e.WithdrawalDate,
                    DropDate = e.DropDate
                    // You would need to join with course, semester, etc. tables for full info
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetEnrolledCoursesForStudent");
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
                _logger.LogError(ex, "Error in CancelEnrollmentRequest");
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
                _logger.LogError(ex, "Error in GetEnrollmentHistoryForStudent");
                throw;
            }
        }

        public async Task<CreditHourStatusDto> GetCreditHourStatus(int studentId)
        {
            try
            {
                var policy = await _enrollmentRepository.GetCreditHourStatus(studentId);

                if (policy == null)
                {
                    throw new Exception("Credit limit policy not found for student");
                }

                // You would need to get current credit hours from student profile
                return new CreditHourStatusDto
                {
                    MaxAllowedCredits = policy.MaxCredits,
                    // CurrentCreditHours and RemainingCredits would need to be calculated
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetCreditHourStatus");
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
                _logger.LogError(ex, "Error in WithdrawFromCourse");
                throw;
            }
        }

        public async Task<IEnumerable<CourseOfferingDto>> GetAssignedCoursesForTeacher(int teacherId)
        {
            try
            {
                var offerings = await _enrollmentRepository.GetAssignedCoursesForTeacher(teacherId);

                return offerings.Select(o => new CourseOfferingDto
                {
                    Id = o.Id
                    // Map other properties as needed
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAssignedCoursesForTeacher");
                throw;
            }
        }

        public async Task<IEnumerable<EnrolledCourseDto>> GetEnrolledStudentsForCourse(int teacherId, int courseOfferingId)
        {
            try
            {
                var enrollments = await _enrollmentRepository.GetEnrolledStudentsForCourse(teacherId, courseOfferingId);

                return enrollments.Select(e => new EnrolledCourseDto
                {
                    EnrollmentId = e.Id,
                    Status = e.Status
                    // Map other properties as needed
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetEnrolledStudentsForCourse");
                throw;
            }
        }

        public async Task<CourseOfferingDto> GetCourseDetailsForTeacher(int teacherId, int courseOfferingId)
        {
            try
            {
                var offering = await _enrollmentRepository.GetCourseDetailsForTeacher(teacherId, courseOfferingId);

                if (offering == null)
                {
                    throw new Exception("Course offering not found");
                }

                return new CourseOfferingDto
                {
                    Id = offering.Id
                    // Map other properties as needed
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetCourseDetailsForTeacher");
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
                _logger.LogError(ex, "Error in GetStudentAcademicHistory");
                throw;
            }
        }

        public async Task<IEnumerable<EnrollmentStatusHistoryDto>> GetEnrollmentStatusHistory(int enrollmentId)
        {
            try
            {
                var history = await _enrollmentRepository.GetEnrollmentStatusHistory(enrollmentId);

                return history.Select(h => new EnrollmentStatusHistoryDto
                {
                    Id = h.Id,
                    OldStatus = h.Status,
                    NewStatus = h.Status, // This would need proper mapping from the history table
                    ChangeDate = h.ModifiedAt,
                    Reason = h.RejectionReason ?? string.Empty
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetEnrollmentStatusHistory");
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
                _logger.LogError(ex, "Error in OverrideCreditLimit");
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
                _logger.LogError(ex, "Error in CheckCoursePrerequisites");
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
                _logger.LogError(ex, "Error in GetStudentCurrentCourseLoad");
                throw;
            }
        }
    }
}