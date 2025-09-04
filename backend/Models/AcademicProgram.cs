namespace backend.Models
{
    public class AcademicProgram : BaseModel
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int DurationSemesters { get; set; } = 8;
        public int CreditHoursRequired { get; set; } = 130;
        public string Description { get; set; }

        // Navigation properties
        public Department? Department { get; set; }
    }
}