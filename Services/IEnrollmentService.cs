using backend.Dtos;
using backend.Models;

namespace backend.Services
{
    public interface IEnrollmentService
    {
        Task<EnrollmentResponseDto> AdminEnrollStudent(EnrollmentRequestDto request, int adminId, int createdBy);
        Task<bool> AdminUnenrollStudent(int studentId, int courseOfferingId, int adminId, int modifiedBy);
        Task<bool> ApproveEnrollment(int enrollmentId, int adminId, int modifiedBy);
        Task<bool> RejectEnrollment(int enrollmentId, int adminId, string rejectionReason, int modifiedBy);
        Task<IEnumerable<EnrollmentResponseDto>> GetPendingEnrollments();
        Task<EnrollmentResponseDto> RequestCourseEnrollment(EnrollmentRequestDto request, int createdBy);
        Task<IEnumerable<CourseOfferingDto>> GetAvailableCoursesForStudent(int studentId);
        Task<IEnumerable<EnrolledCourseDto>> GetEnrolledCoursesForStudent(int studentId);
        Task<bool> CancelEnrollmentRequest(int enrollmentId, int studentId, int modifiedBy);
        Task<IEnumerable<EnrolledCourseDto>> GetEnrollmentHistoryForStudent(int studentId);
        Task<CreditHourStatusDto> GetCreditHourStatus(int studentId);
        Task<bool> WithdrawFromCourse(int enrollmentId, int studentId, int modifiedBy, string reason);
        Task<IEnumerable<CourseOfferingDto>> GetAssignedCoursesForTeacher(int teacherId);
        Task<IEnumerable<EnrolledCourseDto>> GetEnrolledStudentsForCourse(int teacherId, int courseOfferingId);
        Task<CourseOfferingDto> GetCourseDetailsForTeacher(int teacherId, int courseOfferingId);
        Task<StudentProfile> GetStudentAcademicHistory(int teacherId, int studentId);
        Task<IEnumerable<EnrollmentStatusHistoryDto>> GetEnrollmentStatusHistory(int enrollmentId);
        Task<int> OverrideCreditLimit(CreditLimitOverrideDto request, int approvedBy);
        Task<PrerequisiteCheckDto> CheckCoursePrerequisites(int studentId, int courseId);
        Task<CourseLoadDto> GetStudentCurrentCourseLoad(int studentId);
    }
}
