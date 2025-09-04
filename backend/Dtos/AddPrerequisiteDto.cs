using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class AddPrerequisiteDto
    {
        [Required]
        public int CourseId { get; set; }

        [Required]
        public int PrerequisiteCourseId { get; set; }

        public bool IsMandatory { get; set; } = true;

        [Range(1.3, 4.0)]
        public decimal? MinimumGrade { get; set; }
    }
}
