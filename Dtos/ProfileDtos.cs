using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class AdminProfileUpdateDto
    {
        [StringLength(255)]
        public string? Name { get; set; }

        [StringLength(20)]
        public string? Contact { get; set; }

   


    }

    public class AdminProfileUpdateWithFileDto
    {
        public string? Name { get; set; }
        public string? Contact { get; set; }
        
        public IFormFile? ProfilePic { get; set; }
    }

    public class TeacherProfileUpdateDto
    {
        [StringLength(20)]
        public string? Contact { get; set; }

      
       
    }

    public class TeacherQualificationsUpdateDto
    {
        [StringLength(500)]
        public string? Qualification { get; set; }

        [StringLength(255)]
        public string? Specialization { get; set; }

        [Range(0, 50)]
        public int? ExperienceYears { get; set; }
    }

    public class StudentProfileUpdateDto
    {
        [EmailAddress]
        [StringLength(255)]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? Contact { get; set; }

  
       
    }

    public class ProfilePicResponse
    {
        public string? ProfilePicUrl { get; set; }
        public string? ProfilePicType { get; set; }
    }

    public class AdminProfileResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Contact { get; set; }
        public string? ProfilePicUrl { get; set; }
        public string? ProfilePicType { get; set; }
        public string Level { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public string? DepartmentName { get; set; }
    }

    public class TeacherProfileResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Contact { get; set; }
        public string? ProfilePicUrl { get; set; }
        public string? ProfilePicType { get; set; }
        public string Designation { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public string? Qualification { get; set; }
        public string? Specialization { get; set; }
        public int ExperienceYears { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
    }

    public class StudentProfileResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Contact { get; set; }
        public string? ProfilePicUrl { get; set; }
        public string? ProfilePicType { get; set; }
        public string RegNo { get; set; } = string.Empty;
        public int EnrollYear { get; set; }
        public int? CurrentSemester { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string ProgramName { get; set; } = string.Empty;
        public string LevelName { get; set; } = string.Empty;
        public string AcademicStatus { get; set; } = string.Empty;
        public decimal Cgpa { get; set; }
    }
}