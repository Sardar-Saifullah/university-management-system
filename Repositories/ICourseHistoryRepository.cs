using backend.Models;

namespace backend.Repositories
{
    public interface ICourseHistoryRepository
    {

        Task<int> AddCourseHistoryAsync(StudentCourseHistory courseHistory);
        Task<bool> UpdateCourseHistoryAsync(int historyId, StudentCourseHistory courseHistory);
        Task<bool> DeleteCourseHistoryAsync(int historyId, int modifiedBy);
        Task<StudentCourseHistory?> GetCourseHistoryByIdAsync(int id);
        Task<IEnumerable<StudentCourseHistory>> GetCourseHistoryByStudentIdAsync(int studentId);
        Task<IEnumerable<StudentCourseHistory>> GetCourseHistoryByFiltersAsync(int? studentId, int? courseId, int? semesterId,
            string? status, bool? isRetake, string? academicYear, int? limit, int? offset);
        Task<GpaCalculationResult> CalculateStudentGpaAsync(int studentId);
        Task<StudentAcademicSummary> GetStudentAcademicSummaryAsync(int studentId);
        Task<TermGpaResult> CalculateTermGpaAsync(int studentId, int semesterId);
        Task<int> CalculateAllTermGpaForSemesterAsync(int semesterId);
    }
}
