namespace backend.Models
{
    public class TeacherProfile : BaseModel
    {
        public int UserId { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public string Qualification { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public int ExperienceYears { get; set; } = 0;
        public int? ProfilePictureId { get; set; }
    }
}