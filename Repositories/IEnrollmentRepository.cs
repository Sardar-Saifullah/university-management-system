using backend.Dtos;
using backend.Models;

namespace backend.Repositories
{
    public interface IEnrollmentRepository
    {
        Task<int> AdminEnrollStudent(int studentId, int courseOfferingId, int adminId, int createdBy);
        Task<int> AdminUnenrollStudent(int studentId, int courseOfferingId, int adminId, int modifiedBy);
        Task<int> ApproveEnrollment(int enrollmentId, int adminId, int modifiedBy);
        Task<int> RejectEnrollment(int enrollmentId, int adminId, string rejectionReason, int modifiedBy);
        Task<IEnumerable<Enrollment>> GetPendingEnrollments();
        Task<EnrollmentResponseDto> RequestCourseEnrollment(int studentId, int courseOfferingId, int createdBy);
        Task<IEnumerable<CourseOfferingDto>> GetAvailableCoursesForStudent(int studentId);
        Task<IEnumerable<EnrolledCourseDto>> GetEnrolledCoursesForStudent(int studentId);
        Task<int> CancelEnrollmentRequest(int enrollmentId, int studentId, int modifiedBy);
        Task<IEnumerable<Enrollment>> GetEnrollmentHistoryForStudent(int studentId);
        Task<CreditHourStatusDto?> GetCreditHourStatus(int studentId);
        Task<int> WithdrawFromCourse(int enrollmentId, int studentId, int modifiedBy, string reason);
        Task<IEnumerable<CourseOfferingDto>> GetAssignedCoursesForTeacher(int teacherId);
        Task<IEnumerable<EnrolledStudentDto>> GetEnrolledStudentsForCourse(int teacherId, int courseOfferingId);
        Task<CourseOfferingDto?> GetCourseDetailsForTeacher(int teacherId, int courseOfferingId);
        Task<StudentProfile?> GetStudentAcademicHistory(int teacherId, int studentId);
        Task<IEnumerable<EnrollmentStatusHistoryDto>> GetEnrollmentStatusHistory(int enrollmentId);
        Task<int> OverrideCreditLimit(int studentId, int policyId, int newMaxCredits, string reason, int approvedBy, DateTime? expiresAt);
        Task<PrerequisiteCheckDto> CheckCoursePrerequisites(int studentId, int courseId);
        Task<CourseLoadDto?> GetStudentCurrentCourseLoad(int studentId);
    }
}
