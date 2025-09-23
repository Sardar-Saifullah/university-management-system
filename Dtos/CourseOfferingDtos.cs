using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class CourseOfferingCreateDto
    {
        [Required]
        public int CourseId { get; set; }

        [Required]
        public int SemesterId { get; set; }

        [Required]
        [Range(1, 500)]
        public int MaxCapacity { get; set; }

        public DateTime? EnrollmentStart { get; set; }
        public DateTime? EnrollmentEnd { get; set; }
    }

    public class CourseOfferingUpdateDto
    {
        public int? MaxCapacity { get; set; }
        public DateTime? EnrollmentStart { get; set; }
        public DateTime? EnrollmentEnd { get; set; }
        public bool? IsActive { get; set; }
    }

    public class CourseOfferingGetDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseTitle { get; set; } = string.Empty;
        public int SemesterId { get; set; }
        public string SemesterName { get; set; } = string.Empty;
        public DateTime SemesterStartDate { get; set; }
        public DateTime SemesterEndDate { get; set; }
        public int? MaxCapacity { get; set; }
        public int CurrentEnrollment { get; set; }
        public DateTime? EnrollmentStart { get; set; }
        public DateTime? EnrollmentEnd { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public int? ProgramId { get; set; }
        public string ProgramName { get; set; } = string.Empty;
        public int LevelId { get; set; }
        public string LevelName { get; set; } = string.Empty;
        public decimal CreditHours { get; set; }
        public bool IsActive { get; set; }
    }

   
}