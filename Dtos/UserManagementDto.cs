// Add to backend/Dtos/UserManagementDtos.cs

using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class AdminUpdateStudentDto
    {
        [Required]
        public int UserId { get; set; }

        [StringLength(255)]
        public string? Name { get; set; }

        [EmailAddress]
        [StringLength(255)]
        public string? Email { get; set; }

        [StringLength(50)]
        public string? RegNo { get; set; }

        [StringLength(20)]
        public string? Contact { get; set; }

        [Range(2025, 2050)]
        public int? EnrollYear { get; set; }

        [Range(1, 12)]
        public int? CurrentSemester { get; set; } = 1;

        [RegularExpression("active|suspended|graduated|on_leave")]
        public string? AcademicStatus { get; set; }

        [Range(0.00, 4.00)]
        public decimal? Cgpa { get; set; }

        [StringLength(255)]
        public string? ProfilePicUrl { get; set; } = null;
    }

    public class AdminUpdateTeacherDto
    {
        [Required]
        public int UserId { get; set; }

        [StringLength(255)]
        public string? Name { get; set; }

        [EmailAddress]
        [StringLength(255)]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? Contact { get; set; }

        [StringLength(100)]
        public string? Designation { get; set; }

        [StringLength(500)]
        public string? Qualification { get; set; }

        [StringLength(255)]
        public string? Specialization { get; set; }

        [Range(0, 50)]
        public int? ExperienceYears { get; set; }

        [StringLength(255)]
        public string? ProfilePicUrl { get; set; }
    }

    public class AdminGetStudentByRegNoDto
    {
        [Required]
        [StringLength(50)]
        public string RegNo { get; set; } = string.Empty;
    }

    public class AdminGetTeacherByDetailsDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid department ID")]
        public int DepId { get; set; }

        [Required(ErrorMessage = "Designation is required")]
        public string Designation { get; set; } = string.Empty;
    }

    public class AdminGetAllUsersDto
    {
        [Required]
        [RegularExpression("Student|Teacher|Admin")]
        public string ProfileName { get; set; } = string.Empty;

        [Range(1, 100)]
        public int Limit { get; set; } = 10;

        [Range(0, int.MaxValue)]
        public int Offset { get; set; } = 0;
    }

    public class AdminCountUsersDto
    {
        [Required]
        [RegularExpression("Student|Teacher|Admin")]
        public string ProfileName { get; set; } = string.Empty;
    }

    public class AdminDeleteUserDto
    {
        [Required]
        public int UserId { get; set; }
    }

    public class StudentDetailsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public string? ProfilePicUrl { get; set; }
        public string RegNo { get; set; } = string.Empty;
        public int EnrollYear { get; set; }
        public int CurrentSemester { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string ProgramName { get; set; } = string.Empty;
        public string LevelName { get; set; } = string.Empty;
        public string AcademicStatus { get; set; } = string.Empty;
        public decimal Cgpa { get; set; }
        public int TotalSemestersStudied { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class TeacherDetailsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public string? ProfilePicUrl { get; set; }
        public string Designation { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public string Qualification { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public int ExperienceYears { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string DepartmentCode { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}