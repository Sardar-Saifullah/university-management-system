namespace backend.Models
{
    public class Course : BaseModel
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal CreditHours { get; set; } = 3.0m;
        public int LevelId { get; set; }
        public string LevelName { get; set; }
        public int? ProgramId { get; set; }
        public string ProgramName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public bool IsElective { get; set; } = false;
        public int ActiveOfferingsCount { get; set; }
    }
}