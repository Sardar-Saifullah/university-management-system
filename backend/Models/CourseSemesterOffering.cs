namespace backend.Models
{
    public class CourseSemesterOffering : BaseModel
    {
        public int CourseId { get; set; }
        public int SemesterId { get; set; }
        public int? MaxCapacity { get; set; }
        public int CurrentEnrollment { get; set; } = 0;
        public DateTime? EnrollmentStart { get; set; }
        public DateTime? EnrollmentEnd { get; set; }

        // Navigation properties (not stored in DB)
        public string CourseCode { get; set; }
        public string CourseTitle { get; set; }
        public string SemesterName { get; set; }
    }
}