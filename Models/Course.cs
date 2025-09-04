namespace backend.Models
{
    public class Course : BaseModel
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal CreditHours { get; set; } = 3.0m;
        public int LevelId { get; set; }
        public string LevelName { get; set; } // Added to match DTO
        public int? ProgramId { get; set; }
        public string ProgramName { get; set; } // Added to match DTO
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } // Added to match DTO
        public bool IsElective { get; set; } = false;
    }
}