// BaseModels.cs
namespace backend.Models
{
    public class BaseModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
    }

    public class Profile : BaseModel
    {
        public string ProfileName { get; set; } = string.Empty;
    }

    public class User : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? RegNo { get; set; }
        public string Email { get; set; } = string.Empty;
        public int ProfileId { get; set; }
        public string? ProfilePictureType { get; set; }
        public string? ProfilePicUrl { get; set; }
        public string? Contact { get; set; }
    }

    public class Department : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }

    public class Program : BaseModel
    {
        public int DepId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int DurationSemesters { get; set; } = 8;
        public int CreditHoursRequired { get; set; } = 130;
        public string? Description { get; set; }
    }

    public class Semester : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCurrent { get; set; } = false;
        public DateTime? RegistrationStart { get; set; }
        public DateTime? RegistrationEnd { get; set; }
        public string AcademicYear { get; set; } = string.Empty;
    }

    public class Level : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class StudentProfile : BaseModel
    {
        public int UserId { get; set; }
        public string RegNo { get; set; } = string.Empty;
        public int EnrollYear { get; set; }
        public int? CurrentSemester { get; set; }
        public int TotalSemestersStudied { get; set; } = 0;
        public int DepId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public int ProgramId { get; set; }
        public string ProgramName { get; set; } = string.Empty;
        public int LevelId { get; set; }
        public string AcademicStatus { get; set; } = "active";
        public decimal Cgpa { get; set; } = 0.00m;

        public string? PictureType { get; set; }
        public string? PictureUrl { get; set; }
        public string Email { get; set; } = string.Empty;
    }

    public class TeacherProfile : BaseModel
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public int DepId { get; set; }
        public string Designation { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public string? Qualification { get; set; }
        public string? Specialization { get; set; }
        public int ExperienceYears { get; set; } = 0;

        public string? PictureUrl { get; set; }
        public string? PictureType { get; set; }
    }

    public class AdminProfile : BaseModel
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Level { get; set; } = "super_admin";
        public string? AccessLevel { get; set; }
        public DateTime HireDate { get; set; }
        public int? DepartmentId { get; set; }
        public string? PictureUrl { get; set; }
        public string? PictureType { get; set; }
    }

    public class UserSession : BaseModel
    {
        public string SessionId { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Jti { get; set; } = string.Empty;
        public string? DeviceInfo { get; set; }
        public DateTime LoginAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; } = false;
        public DateTime? RevokedAt { get; set; }
        public DateTime LastActivity { get; set; } = DateTime.UtcNow;
    }

    public class Activity : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }

    public class ActivityProfileMapping : BaseModel
    {
        public int ProfileId { get; set; }
        public int ActivityId { get; set; }
        public bool CanCreate { get; set; } = false;
        public bool CanRead { get; set; } = false;
        public bool CanUpdate { get; set; } = false;
        public bool CanDelete { get; set; } = false;
        public bool CanExport { get; set; } = false;
    }

    // Add to your existing BaseModels.cs

    public class AdminUpdateStudentModel : BaseModel
    {
        public int AdminId { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? RegNo { get; set; }
        public string? Contact { get; set; }
        public int? EnrollYear { get; set; }
        public int? CurrentSemester { get; set; }
        public string? AcademicStatus { get; set; }
        public decimal? Cgpa { get; set; }
        public string? ProfilePicUrl { get; set; }
    }

    public class AdminUpdateTeacherModel : BaseModel
    {
        public int AdminId { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Contact { get; set; }
        public string? Designation { get; set; }
        public string? Qualification { get; set; }
        public string? Specialization { get; set; }
        public int? ExperienceYears { get; set; }
        public string? ProfilePicUrl { get; set; }
    }

    public class AdminDeleteUserModel : BaseModel
    {
        public int AdminId { get; set; }
        public int UserId { get; set; }
    }

    public class AdminCountUsersModel : BaseModel
    {
        public int AdminId { get; set; }
        public string ProfileName { get; set; } = string.Empty;
    }

    public class AdminGetTeacherModel : BaseModel
    {
        public int AdminId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DepId { get; set; }
        public string Designation { get; set; } = string.Empty;
    }
}