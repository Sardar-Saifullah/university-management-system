namespace backend.Models
{
    public class StudentCourseHistory : BaseModel
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int SemesterId { get; set; }
        public int? EnrollmentId { get; set; }
        public decimal? Grade { get; set; }
        public string LetterGrade { get; set; }
        public decimal CreditsEarned { get; set; }
        public decimal? TermGpa { get; set; }
        public string Status { get; set; } = "in_progress";
        public bool IsRetake { get; set; } = false;
        public int? OriginalAttemptId { get; set; }
        public int? ModifiedBy { get; set; }
    }

    public class GpaCalculationResult
    {
        public decimal Cgpa { get; set; }
        public decimal TotalCreditsEarned { get; set; }
        public decimal TotalCreditsAttempted { get; set; }
        public decimal TotalGradePoints { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class StudentAcademicSummary
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

    public class TermGpaResult
    {
        public decimal? TermGpa { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}