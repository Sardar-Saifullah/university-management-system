using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class TeacherProfileDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string ProfilePicUrl { get; set; }
        public string ProfilePicType { get; set; }
        public string Designation { get; set; }
        public DateTime HireDate { get; set; }
        public string Qualification { get; set; }
        public string Specialization { get; set; }
        public int ExperienceYears { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }

    public class TeacherOwnProfileUpdateDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
    }

    public class TeacherQualificationUpdateDto
    {
        public string Qualification { get; set; }
        public string Specialization { get; set; }

        [Range(0, 50)]
        public int ExperienceYears { get; set; }
    }
}


