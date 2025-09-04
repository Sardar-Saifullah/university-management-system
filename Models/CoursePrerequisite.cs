namespace backend.Models
{
    public class CoursePrerequisite : BaseModel
    {
        public int CourseId { get; set; }
        public int PrerequisiteCourseId { get; set; }
        public bool IsMandatory { get; set; } = true;
        public decimal? MinimumGrade { get; set; }
        public int? ModifiedBy { get; set; }
    }

    public class PrerequisiteValidationResult
    {
        public bool IsEligible { get; set; }
        public List<MissingPrerequisite> MissingPrerequisites { get; set; } = new();
    }

    public class MissingPrerequisite
    {
        public int PrerequisiteCourseId { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseTitle { get; set; } = string.Empty;
        public decimal? RequiredGrade { get; set; }
        public decimal? StudentGrade { get; set; }
        public bool IsMet { get; set; }
    }
}