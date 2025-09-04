namespace backend.Models
{
    public class StudentProfile : BaseModel
    {
        public int UserId { get; set; }
        public string RegistrationNo { get; set; }
        public int EnrollYear { get; set; }
        public int? CurrentSemester { get; set; }
        public int TotalSemestersStudied { get; set; } = 0;
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int ProgramId { get; set; }
        public string ProgramName { get; set; }
        public int LevelId { get; set; }
        public string LevelName { get; set; }
        public string AcademicStatus { get; set; } = "active";
        public decimal Cgpa { get; set; } = 0.00m;
        public decimal CurrentCreditHours { get; set; } = 0.00m;
        public decimal CompletedCreditHours { get; set; } = 0.00m;
        public decimal AttemptedCreditHours { get; set; } = 0.00m;
        public int? ProfilePictureId { get; set; }
    }
}