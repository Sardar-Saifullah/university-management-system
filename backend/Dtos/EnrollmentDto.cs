// Dtos/Enrollment/EnrollmentRequestDto.cs
using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class EnrollmentRequestDto
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseOfferingId { get; set; }
    }

    public class EnrollmentApprovalDto
    {
        public string? RejectionReason { get; set; }
    }

    public class EnrollmentResponseDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseOfferingId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string? RejectionReason { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class CourseOfferingDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal CreditHours { get; set; }
        public bool IsElective { get; set; }
        public int? MaxCapacity { get; set; }
        public int CurrentEnrollment { get; set; }
        public int AvailableSeats { get; set; }
        public string SemesterName { get; set; } = string.Empty;
        public string AcademicYear { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string ProgramName { get; set; } = string.Empty;
        public string LevelName { get; set; } = string.Empty;
        public string PrerequisiteCourses { get; set; } = string.Empty;
    }

    public class EnrolledCourseDto
    {
        public int EnrollmentId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string? RejectionReason { get; set; }
        public DateTime? WithdrawalDate { get; set; }
        public DateTime? DropDate { get; set; }
        public int CourseId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal CreditHours { get; set; }
        public string SemesterName { get; set; } = string.Empty;
        public string AcademicYear { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string ProgramName { get; set; } = string.Empty;
        public string LevelName { get; set; } = string.Empty;
        public string ApprovedByName { get; set; } = string.Empty;
    }

    public class CreditHourStatusDto
    {
        public decimal CurrentCreditHours { get; set; }
        public int MaxAllowedCredits { get; set; }
        public decimal RemainingCredits { get; set; }
        public string? OverrideReason { get; set; }
        public DateTime? OverrideExpiry { get; set; }
        public string? ApprovedBy { get; set; }
    }

    public class WithdrawalRequestDto
    {
        [Required]
        public string Reason { get; set; } = string.Empty;
    }

    public class PrerequisiteCheckDto
    {
        public bool PrerequisitesMet { get; set; }
        public string MissingPrerequisites { get; set; } = string.Empty;
    }

    public class CourseLoadDto
    {
        public int StudentId { get; set; }
        public string RegNo { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public decimal CurrentCreditHours { get; set; }
        public int CreditLimit { get; set; }
        public decimal RemainingCredits { get; set; }
        public string CurrentCourses { get; set; } = string.Empty;
    }
    public class EnrollmentStatusHistoryDto
    {
        public int Id { get; set; }
        public string OldStatus { get; set; } = string.Empty;
        public string NewStatus { get; set; } = string.Empty;
        public DateTime ChangeDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string ChangedBy { get; set; } = string.Empty;
    }

    public class CreditLimitOverrideDto
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int PolicyId { get; set; }

        [Required]
        [Range(1, 30)]
        public int NewMaxCredits { get; set; }

        [Required]
        [StringLength(500)]
        public string Reason { get; set; } = string.Empty;

        public DateTime? ExpiresAt { get; set; }
    }

}