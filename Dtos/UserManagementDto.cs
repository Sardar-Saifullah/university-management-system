using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace backend.Dtos
{
    public class PaginationRequestDto
    {
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 20;

        public string? SearchTerm { get; set; }
    }

    public class PaginationResponseDto<T>
    {
        public List<T> Data { get; set; } = new();
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }

    public class UserCreateDto
    {
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int ProfileId { get; set; }

        [Phone]
        [StringLength(20)]
        public string? Contact { get; set; }
    }

    public class UserUpdateDto
    {
        [StringLength(255, MinimumLength = 2)]
        public string? Name { get; set; }

        [EmailAddress]
        [StringLength(255)]
        public string? Email { get; set; }

        [Range(1, int.MaxValue)]
        public int? ProfileId { get; set; }

        [Phone]
        [StringLength(20)]
        public string? Contact { get; set; }

        public bool? IsActive { get; set; }
    }

    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int ProfileId { get; set; }
        public string ProfileName { get; set; } = string.Empty;
        public string? Contact { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool IsActive { get; set; }
    }

    public class StudentProfileCreateDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string RegNo { get; set; } = string.Empty;

        [Required]
        [Range(2000, 2100)]
        public int EnrollYear { get; set; }

        [Range(1, int.MaxValue)]
        public int? CurrentSemester { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int DepId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int ProgramId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int LevelId { get; set; }

        [Range(0, 4)]
        public decimal Cgpa { get; set; }
    }

    public class StudentProfileUpdateDto
    {
        [StringLength(50)]
        public string? RegNo { get; set; }

        [Range(2000, 2100)]
        public int? EnrollYear { get; set; }

        [Range(1, int.MaxValue)]
        public int? CurrentSemester { get; set; }

        [Range(1, int.MaxValue)]
        public int? DepId { get; set; }

        [Range(1, int.MaxValue)]
        public int? ProgramId { get; set; }

        [Range(1, int.MaxValue)]
        public int? LevelId { get; set; }

        [StringLength(20)]
        public string? AcademicStatus { get; set; }

        [Range(0, 4)]
        public decimal? Cgpa { get; set; }

        [Range(0, 200)]
        public decimal? CurrentCreditHours { get; set; }

        [Range(0, 200)]
        public decimal? CompletedCreditHours { get; set; }
        [Range(0, 200)]
        public decimal? AttemptedCreditHours { get; set; }
    }

    public class StudentProfileResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string RegNo { get; set; } = string.Empty;
        public int EnrollYear { get; set; }
        public int? CurrentSemester { get; set; }
        public int DepId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public int ProgramId { get; set; }
        public string ProgramName { get; set; } = string.Empty;
        public int LevelId { get; set; }
        public string LevelName { get; set; } = string.Empty;
        public string AcademicStatus { get; set; } = string.Empty;
        public decimal Cgpa { get; set; }
        public decimal CurrentCreditHours { get; set; }
        public decimal CompletedCreditHours { get; set; }
        public decimal AttemptedCreditHours { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }

    public class TeacherProfileCreateDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int DepId { get; set; }

        [Required]
        [StringLength(100)]
        public string Designation { get; set; } = string.Empty;

        [Required]
        public DateTime? HireDate { get; set; }

        [StringLength(500)]
        public string? Qualification { get; set; }

        [StringLength(255)]
        public string? Specialization { get; set; }

        [Range(0, 50)]
        public int ExperienceYears { get; set; }
    }

    public class TeacherProfileUpdateDto
    {
        [Range(1, int.MaxValue)]
        public int? DepId { get; set; }

        [StringLength(100)]
        public string? Designation { get; set; }

        public DateTime? HireDate { get; set; }

        [StringLength(500)]
        public string? Qualification { get; set; }

        [StringLength(255)]
        public string? Specialization { get; set; }

        [Range(0, 50)]
        public int? ExperienceYears { get; set; }
    }

    public class TeacherProfileResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int DepId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public string? Qualification { get; set; }
        public string? Specialization { get; set; }
        public int ExperienceYears { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }

    public class AdminProfileCreateDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Required]
        [StringLength(20)]
        public string Level { get; set; } = string.Empty;

        public JsonDocument? AccessLevel { get; set; }

        [Required]
        public DateOnly HireDate { get; set; }

   
        public int? DepartmentId { get; set; }
    }

    public class AdminProfileUpdateDto
    {
        [StringLength(20)]
        public string? Level { get; set; }

        
        public JsonDocument? AccessLevel { get; set; }

        public DateTime? HireDate { get; set; }

        
        public int? DepartmentId { get; set; }
    }

    public class AdminProfileResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public JsonDocument? AccessLevel { get; set; }
        public DateTime HireDate { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}