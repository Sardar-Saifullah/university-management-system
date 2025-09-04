using System.ComponentModel.DataAnnotations;
namespace backend.Dtos
{
    
        public class LoginRequest
    {
            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email format")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Password is required")]
            [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;
        }

    public class StudentLoginRequest
    {
            [Required(ErrorMessage = "Registration number is required")]
            [StringLength(20, ErrorMessage = "Registration number must be at most 20 characters")]
            public string RegistrationNo { get; set; } = string.Empty;

            [Required(ErrorMessage = "Password is required")]
            [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;
        }
    public class LoginUserResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ProfileName { get; set; }
        public string ProfilePicUrl { get; set; }// This now maps to file_path
        public string ProfilePictureType { get; set; }// This now maps to mime_type
        public string RegistrationNo { get; set; } // For student
        public string ProgramName { get; set; } // For student
        public string Designation { get; set; } // For teacher
    }
    public class RevokeSessionRequest
    {
        [Required(ErrorMessage = "Session ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Session ID")]
        public int SessionId { get; set; }
    }
    public class AuthResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Login successful";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Profile { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string? ProfilePicUrl { get; set; }
        public string? ProfilePicType { get; set; }
        public string SessionId { get; set; } = string.Empty;  // Added
        public DateTime ExpiresAt { get; set; }  // Added
        public string? RegistrationNo { get; set; } // For student
        public string? ProgramName { get; set; } // For student
        public string? Designation { get; set; } // For teacher
        public List<PermissionResponse> Permissions { get; set; } = new List<PermissionResponse>();
    }

    public class StudentRegistrationDto
        {
            [Required(ErrorMessage = "Name is required")]
            [StringLength(255, ErrorMessage = "Name must be less than 255 characters")]
            public string Name { get; set; } = string.Empty;

            [Required(ErrorMessage = "Password is required")]
            [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email format")]
            [StringLength(255, ErrorMessage = "Email must be less than 255 characters")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Registration number is required")]
            [StringLength(50, ErrorMessage = "Registration number must be less than 50 characters")]
            public string RegistrationNo { get; set; } = string.Empty;

            [Required(ErrorMessage = "Enrollment year is required")]
            [Range(2000, 2100, ErrorMessage = "Invalid enrollment year")]
            public int EnrollYear { get; set; }

            [Required(ErrorMessage = "Department ID is required")]
            [Range(1, int.MaxValue, ErrorMessage = "Invalid department ID")]
            public int DepartmentId { get; set; }

            [Required(ErrorMessage = "Program ID is required")]
            [Range(1, int.MaxValue, ErrorMessage = "Invalid program ID")]
            public int ProgramId { get; set; }

            [Required(ErrorMessage = "Level ID is required")]
            [Range(1, int.MaxValue, ErrorMessage = "Invalid level ID")]
            public int LevelId { get; set; }

        [StringLength(20, ErrorMessage = "Contact must be less than 20 characters")]
        public string? Contact { get; set; }

        [Required(ErrorMessage = "Created by is required")]
            public int CreatedBy { get; set; }
    }

    public class TeacherRegistrationDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(255, ErrorMessage = "Name must be less than 255 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(255, ErrorMessage = "Email must be less than 255 characters")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid department ID")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Designation is required")]
        [StringLength(100, ErrorMessage = "Designation must be less than 100 characters")]
        public string Designation { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hire date is required")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        [StringLength(500, ErrorMessage = "Qualification must be less than 500 characters")]
        public string? Qualification { get; set; }

        // FIXED: Move StringLength attribute to Specialization (which is a string)
        [StringLength(255, ErrorMessage = "Specialization must be less than 255 characters")]
        public string? Specialization { get; set; }

        [Required(ErrorMessage = "Experience is required")]
        [Range(0, 50, ErrorMessage = "Experience must be between 0 and 50 years")]
        public int ExperienceYears { get; set; }

        [StringLength(20, ErrorMessage = "Contact must be less than 20 characters")]
        public string? Contact { get; set; }

        public int CreatedBy { get; set; }
    }

    public class AdminRegistrationDto
        {
            [Required(ErrorMessage = "Name is required")]
            [StringLength(255, ErrorMessage = "Name must be less than 255 characters")]
            public string Name { get; set; } = string.Empty;

            [Required(ErrorMessage = "Password is required")]
            [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email format")]
            [StringLength(255, ErrorMessage = "Email must be less than 255 characters")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Level is required")]
            [RegularExpression("^(super_admin|admin|moderator)$", ErrorMessage = "Invalid admin level")]
            public string Level { get; set; } = string.Empty;

            [Required(ErrorMessage = "Hire date is required")]
            [DataType(DataType.Date)]
            public DateTime HireDate { get; set; }

        [StringLength(20, ErrorMessage = "Contact must be less than 20 characters")]
        public string? Contact { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Invalid department ID")]
            public int? DepartmentId { get; set; }

            public int CreatedBy { get; set; }
    }

    public class TeacherRegistrationResponse
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
   
        public string? Department { get; set; } = string.Empty;
        public string? Qualification { get; set; } 
        public DateTime HireDate { get; set; } 
        public string? Designation { get; set; } = string.Empty;
        public int ExperienceYears { get; set; }
        public string? Specialization { get; set; } = string.Empty;
    }
    public class StudentRegistrationResponse
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? RegistrationNo { get; set; }
        public string? Department { get; set; }
        public string? Program { get; set; }
   
        public string? Level { get; set; }
    }

    public class AdminRegistrationResponse
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Department { get; set; }
        public string? Level { get; set; }
        public DateTime HireDate { get; set; }
    }
    public class UserResponse
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Profile { get; set; } = string.Empty;
            public string AdditionalInfo { get; set; } = string.Empty;
        }

        public class UserProfileResponse
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string? Contact { get; set; }
            public string RoleSpecificInfo { get; set; } = string.Empty;
        }

        public class ActiveSessionResponse
        {
            public int SessionId { get; set; }
            public int UserId { get; set; }
            public string UserName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Profile { get; set; } = string.Empty;
            public DateTime LoginAt { get; set; }
            public DateTime ExpiresAt { get; set; }
            public string DeviceInfo { get; set; } = string.Empty;
            public DateTime LastActivity { get; set; }
        }

        public class PermissionResponse
        {
            public int ProfileId { get; set; }
            public string ProfileName { get; set; } = string.Empty;
            public int ActivityId { get; set; }
            public string ActivityName { get; set; } = string.Empty;
            public string ActivityUrl { get; set; } = string.Empty;
            public bool CanCreate { get; set; }
            public bool CanRead { get; set; }
            public bool CanUpdate { get; set; }
            public bool CanDelete { get; set; }
            public bool CanExport { get; set; }
        }


    public class TerminateSessionsResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int SessionsTerminated { get; set; } // Useful for terminate-others

        public TerminateSessionsResponse(bool success, string message, int sessionsTerminated = 0)
        {
            Success = success;
            Message = message;
            SessionsTerminated = sessionsTerminated;
        }
    }

}