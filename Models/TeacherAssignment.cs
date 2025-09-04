namespace backend.Models
{
    public class TeacherAssignment : BaseModel
    {
        public int TeacherId { get; set; }
        public int CourseOfferingId { get; set; }
        public bool IsPrimary { get; set; } = true;
        public int? ModifiedBy { get; set; }
    }

    public class TeacherAssignmentDetail : TeacherAssignment
    {
        public string TeacherName { get; set; }
        public string DepartmentName { get; set; }
        public string CourseCode { get; set; }
        public string CourseTitle { get; set; }
        public string SemesterName { get; set; }
        public DateTime SemesterStart { get; set; }
        public DateTime SemesterEnd { get; set; }
    }
}