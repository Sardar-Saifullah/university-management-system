using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class StudentProfileDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string ProfilePicUrl { get; set; }
        public string ProfilePicType { get; set; }
        public string RegNo { get; set; }
        public int EnrollYear { get; set; }
        public int? CurrentSemester { get; set; }
        public string DepartmentName { get; set; }
        public string ProgramName { get; set; }
        public string AcademicStatus { get; set; }
        public decimal Cgpa { get; set; }
        public decimal CurrentCreditHours { get; set; }
        public decimal CompletedCreditHours { get; set; }
        public decimal AttemptedCreditHours { get; set; }
        public string LevelName { get; set; }
        public string CurrentSemesterName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
    public class StudentOwnProfileUpdateDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
    }

}
