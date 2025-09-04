// Dtos/CourseHistory/CourseHistoryCreateDto.cs
using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class CourseHistoryCreateDto
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public int SemesterId { get; set; }

        public int? EnrollmentId { get; set; }

        [Range(0.00, 4.00)]
        public decimal? Grade { get; set; }

        [StringLength(2)]
        public string? LetterGrade { get; set; }

        [Required]
        [Range(0.00, 6.00)]
        public decimal CreditsEarned { get; set; }

        [Range(0.00, 4.00)]
        public decimal? TermGpa { get; set; }

        [Required]
        [RegularExpression("completed|failed|withdrawn|in_progress")]
        public string Status { get; set; } = "in_progress";

        public bool IsRetake { get; set; } = false;

        public int? OriginalAttemptId { get; set; }
    }

    public class CourseHistoryUpdateDto
    {
        [Range(0.00, 4.00)]
        public decimal? Grade { get; set; }

        [StringLength(2)]
        public string? LetterGrade { get; set; }

        [Range(0.00, 6.00)]
        public decimal? CreditsEarned { get; set; }

        [Range(0.00, 4.00)]
        public decimal? TermGpa { get; set; }

        [RegularExpression("completed|failed|withdrawn|in_progress")]
        public string? Status { get; set; }

        public bool? IsRetake { get; set; }

        public int? OriginalAttemptId { get; set; }
    }
    public class CourseHistoryResponseDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string RegNo { get; set; } = string.Empty;
        public int? CurrentSemester { get; set; }
        public decimal Cgpa { get; set; }
        public int CourseId { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseTitle { get; set; } = string.Empty;
        public decimal CourseCreditHours { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public int SemesterId { get; set; }
        public string SemesterName { get; set; } = string.Empty;
        public string AcademicYear { get; set; } = string.Empty;
        public int? EnrollmentId { get; set; }
        public decimal? Grade { get; set; }
        public string? LetterGrade { get; set; }
        public decimal CreditsEarned { get; set; }
        public decimal? TermGpa { get; set; }
        public string Status { get; set; } = "in_progress";
        public bool IsRetake { get; set; }
        public int? OriginalAttemptId { get; set; }
        public decimal? OriginalGrade { get; set; }
        public string? OriginalLetterGrade { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string AttemptType { get; set; } = "First Attempt";
    }

    public class StudentAcademicSummaryDto
    {
        public int StudentId { get; set; }
        public string RegNo { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int EnrollYear { get; set; }
        public int? CurrentSemester { get; set; }
        public int TotalSemestersStudied { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string DepartmentCode { get; set; } = string.Empty;
        public string ProgramName { get; set; } = string.Empty;
        public string ProgramCode { get; set; } = string.Empty;
        public int DurationSemesters { get; set; }
        public int CreditHoursRequired { get; set; }
        public string LevelName { get; set; } = string.Empty;
        public string AcademicStatus { get; set; } = "active";
        public decimal Cgpa { get; set; }
        public decimal CurrentCreditHours { get; set; }
        public decimal CompletedCreditHours { get; set; }
        public decimal AttemptedCreditHours { get; set; }
        public decimal CompletionPercentage { get; set; }
        public int TotalCoursesTaken { get; set; }
        public int CoursesCompleted { get; set; }
        public int CoursesFailed { get; set; }
        public int CoursesRetaken { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
    public class GpaCalculationResultDto
    {
        public decimal Cgpa { get; set; }
        public decimal TotalCreditsEarned { get; set; }
        public decimal TotalCreditsAttempted { get; set; }
        public decimal TotalGradePoints { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class TermGpaResultDto
    {
        public decimal? TermGpa { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}