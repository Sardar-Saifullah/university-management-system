// Services/Implementations/CourseHistoryService.cs
using backend.Dtos;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class CourseHistoryService : ICourseHistoryService
    {
        private readonly ICourseHistoryRepository _courseHistoryRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ISemesterRepository _semesterRepository;

        public CourseHistoryService(ICourseHistoryRepository courseHistoryRepository, IProfileRepository profileRepository, ICourseRepository courseRepository, ISemesterRepository semesterRepository )
        {
            _courseHistoryRepository = courseHistoryRepository;
            _profileRepository = profileRepository;

            _courseRepository = courseRepository;
            _semesterRepository = semesterRepository;
        }

        public async Task<CourseHistoryResponseDto> AddCourseHistoryAsync(CourseHistoryCreateDto dto, int createdBy)
        {
            var courseHistory = new StudentCourseHistory
            {
                StudentId = dto.StudentId,
                CourseId = dto.CourseId,
                SemesterId = dto.SemesterId,
                EnrollmentId = dto.EnrollmentId,
                Grade = dto.Grade,
                LetterGrade = dto.LetterGrade,
                CreditsEarned = dto.CreditsEarned,
                TermGpa = dto.TermGpa,
                Status = dto.Status,
                IsRetake = dto.IsRetake,
                OriginalAttemptId = dto.OriginalAttemptId,
                CreatedBy = createdBy
            };

            var id = await _courseHistoryRepository.AddCourseHistoryAsync(courseHistory);
            var result = await _courseHistoryRepository.GetCourseHistoryByIdAsync(id);

            return await MapToCourseHistoryResponseDto(result!);
        }

        public async Task<CourseHistoryResponseDto> UpdateCourseHistoryAsync(int historyId, CourseHistoryUpdateDto dto, int modifiedBy)
        {
            var existingHistory = await _courseHistoryRepository.GetCourseHistoryByIdAsync(historyId);
            if (existingHistory == null)
            {
                throw new Exception("Course history record not found");
            }

            // Update only the provided fields
            if (dto.Grade.HasValue) existingHistory.Grade = dto.Grade;
            if (dto.LetterGrade != null) existingHistory.LetterGrade = dto.LetterGrade;
            if (dto.CreditsEarned.HasValue) existingHistory.CreditsEarned = dto.CreditsEarned.Value;
            if (dto.TermGpa.HasValue) existingHistory.TermGpa = dto.TermGpa;
            if (dto.Status != null) existingHistory.Status = dto.Status;
            if (dto.IsRetake.HasValue) existingHistory.IsRetake = dto.IsRetake.Value;
            if (dto.OriginalAttemptId.HasValue) existingHistory.OriginalAttemptId = dto.OriginalAttemptId;

            existingHistory.ModifiedBy = modifiedBy;

            await _courseHistoryRepository.UpdateCourseHistoryAsync(historyId, existingHistory);
            var updatedHistory = await _courseHistoryRepository.GetCourseHistoryByIdAsync(historyId);

            return await MapToCourseHistoryResponseDto(updatedHistory!);
        }

        public async Task<bool> DeleteCourseHistoryAsync(int historyId, int modifiedBy)
        {
            return await _courseHistoryRepository.DeleteCourseHistoryAsync(historyId, modifiedBy);
        }

        public async Task<CourseHistoryResponseDto> GetCourseHistoryByIdAsync(int id)
        {
            var history = await _courseHistoryRepository.GetCourseHistoryByIdAsync(id);
            if (history == null)
            {
                throw new Exception("Course history record not found");
            }

            return await MapToCourseHistoryResponseDto(history);
        }

        public async Task<IEnumerable<CourseHistoryResponseDto>> GetCourseHistoryByStudentIdAsync(int studentId)
        {
            var histories = await _courseHistoryRepository.GetCourseHistoryByStudentIdAsync(studentId);
            var result = new List<CourseHistoryResponseDto>();

            foreach (var history in histories)
            {
                result.Add(await MapToCourseHistoryResponseDto(history));
            }

            return result;
        }

        public async Task<IEnumerable<CourseHistoryResponseDto>> GetCourseHistoryByFiltersAsync(int? studentId, int? courseId, int? semesterId,
            string? status, bool? isRetake, string? academicYear, int? limit, int? offset)
        {
            var histories = await _courseHistoryRepository.GetCourseHistoryByFiltersAsync(studentId, courseId, semesterId,
                status, isRetake, academicYear, limit, offset);

            var result = new List<CourseHistoryResponseDto>();

            foreach (var history in histories)
            {
                result.Add(await MapToCourseHistoryResponseDto(history));
            }

            return result;
        }

        public async Task<GpaCalculationResultDto> CalculateStudentGpaAsync(int studentId)
        {
            var result = await _courseHistoryRepository.CalculateStudentGpaAsync(studentId);

            return new GpaCalculationResultDto
            {
                Cgpa = result.Cgpa,
                TotalCreditsEarned = result.TotalCreditsEarned,
                TotalCreditsAttempted = result.TotalCreditsAttempted,
                TotalGradePoints = result.TotalGradePoints,
                Message = result.Message
            };
        }

        public async Task<StudentAcademicSummaryDto> GetStudentAcademicSummaryAsync(int studentId)
        {
            var result = await _courseHistoryRepository.GetStudentAcademicSummaryAsync(studentId);

            return new StudentAcademicSummaryDto
            {
                StudentId = result.StudentId,
                RegNo = result.RegNo,
                Name = result.Name,
                EnrollYear = result.EnrollYear,
                CurrentSemester = result.CurrentSemester,
                TotalSemestersStudied = result.TotalSemestersStudied,
                DepartmentName = result.DepartmentName,
                DepartmentCode = result.DepartmentCode,
                ProgramName = result.ProgramName,
                ProgramCode = result.ProgramCode,
                DurationSemesters = result.DurationSemesters,
                CreditHoursRequired = result.CreditHoursRequired,
                LevelName = result.LevelName,
                AcademicStatus = result.AcademicStatus,
                Cgpa = result.Cgpa,
                CurrentCreditHours = result.CurrentCreditHours,
                CompletedCreditHours = result.CompletedCreditHours,
                AttemptedCreditHours = result.AttemptedCreditHours,
                CompletionPercentage = result.CompletionPercentage,
                TotalCoursesTaken = result.TotalCoursesTaken,
                CoursesCompleted = result.CoursesCompleted,
                CoursesFailed = result.CoursesFailed,
                CoursesRetaken = result.CoursesRetaken,
                CreatedAt = result.CreatedAt,
                ModifiedAt = result.ModifiedAt
            };
        }

        public async Task<TermGpaResultDto> CalculateTermGpaAsync(int studentId, int semesterId)
        {
            var result = await _courseHistoryRepository.CalculateTermGpaAsync(studentId, semesterId);

            return new TermGpaResultDto
            {
                TermGpa = result.TermGpa,
                Message = result.Message
            };
        }

        public async Task<int> CalculateAllTermGpaForSemesterAsync(int semesterId)
        {
            return await _courseHistoryRepository.CalculateAllTermGpaForSemesterAsync(semesterId);
        }

        private async Task<CourseHistoryResponseDto> MapToCourseHistoryResponseDto(StudentCourseHistory history)
        {
            // Get additional information from related tables
            var student = await _profileRepository.GetStudentProfile(history.StudentId);
            var course = await _courseRepository.GetCourseById(history.CourseId);
            var semester = await _semesterRepository.GetById(history.SemesterId);

            StudentCourseHistory? originalAttempt = null;
            if (history.OriginalAttemptId.HasValue)
            {
                originalAttempt = await _courseHistoryRepository.GetCourseHistoryByIdAsync(history.OriginalAttemptId.Value);
            }

            return new CourseHistoryResponseDto
            {
                Id = history.Id,
                StudentId = history.StudentId,
                StudentName = student?.Name ?? string.Empty,
                RegNo = student?.RegNo ?? string.Empty,
                CurrentSemester = student?.CurrentSemester,
                Cgpa = student?.Cgpa ?? 0,
                CourseId = history.CourseId,
                CourseCode = course?.Code ?? string.Empty,
                CourseTitle = course?.Title ?? string.Empty,
                CourseCreditHours = course?.CreditHours ?? 0,
                DepartmentName = course?.DepartmentName ?? string.Empty,
                SemesterId = history.SemesterId,
                SemesterName = semester?.Name ?? string.Empty,
                AcademicYear = semester?.AcademicYear ?? string.Empty,
                EnrollmentId = history.EnrollmentId,
                Grade = history.Grade,
                LetterGrade = history.LetterGrade,
                CreditsEarned = history.CreditsEarned,
                TermGpa = history.TermGpa,
                Status = history.Status,
                IsRetake = history.IsRetake,
                OriginalAttemptId = history.OriginalAttemptId,
                OriginalGrade = originalAttempt?.Grade,
                OriginalLetterGrade = originalAttempt?.LetterGrade,
                CreatedAt = history.CreatedAt,
                ModifiedAt = history.ModifiedAt,
                AttemptType = history.IsRetake && history.OriginalAttemptId.HasValue ? "Retake" : "First Attempt"
            };
        }
    }
}