namespace backend.Models
{
    public class Semester : BaseModel
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCurrent { get; set; } = false;
        public DateTime? RegistrationStart { get; set; }
        public DateTime? RegistrationEnd { get; set; }
        public string AcademicYear { get; set; }
    }
}