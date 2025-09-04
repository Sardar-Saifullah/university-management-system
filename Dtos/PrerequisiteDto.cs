using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class PrerequisiteDto
    {
        [Required]
        public int CourseId { get; set; }
        [Required]
        public int PrerequisiteCourseId { get; set; }
        public string PrerequisiteCode { get; set; } = string.Empty;
        public string PrerequisiteTitle { get; set; } = string.Empty;
        public string PrerequisiteLevel { get; set; } = string.Empty;
        public decimal PrerequisiteCredits { get; set; }
        public bool IsMandatory { get; set; }
        [Range(0.0, 4.0)]
        public decimal? MinimumGrade { get; set; }
        public string GradeRequirementStatus { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
