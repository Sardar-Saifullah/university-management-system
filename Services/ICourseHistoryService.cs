using backend.Dtos;

namespace backend.Services
{
    public interface ICourseHistoryService
    {

        Task<CourseHistoryResponseDto> AddCourseHistoryAsync(CourseHistoryCreateDto dto, int createdBy);
        Task<CourseHistoryResponseDto> UpdateCourseHistoryAsync(int historyId, CourseHistoryUpdateDto dto, int modifiedBy);
        Task<bool> DeleteCourseHistoryAsync(int historyId, int modifiedBy);
        Task<CourseHistoryResponseDto> GetCourseHistoryByIdAsync(int id);
        Task<IEnumerable<CourseHistoryResponseDto>> GetCourseHistoryByStudentIdAsync(int studentId);
        Task<IEnumerable<CourseHistoryResponseDto>> GetCourseHistoryByFiltersAsync(int? studentId, int? courseId, int? semesterId,
            string? status, bool? isRetake, string? academicYear, int? limit, int? offset);
        Task<GpaCalculationResultDto> CalculateStudentGpaAsync(int studentId);
        Task<StudentAcademicSummaryDto> GetStudentAcademicSummaryAsync(int studentId);
        Task<TermGpaResultDto> CalculateTermGpaAsync(int studentId, int semesterId);
        Task<int> CalculateAllTermGpaForSemesterAsync(int semesterId);
    }
}
