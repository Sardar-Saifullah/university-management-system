using backend.Dtos;
using backend.Models;
using backend.Repositories;
using backend.Utilities;
using System.Data;

namespace backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserResponse> RegisterStudent(StudentRegistrationDto dto, int createdBy)
        {
            var passwordHash = _passwordHasher.HashPassword(dto.Password);
            dto.Password = passwordHash;

            var result = await _userRepository.RegisterStudent(dto, createdBy);
            return MapUserResponse(result.Rows[0]);
        }

        public async Task<UserResponse> RegisterTeacher(TeacherRegistrationDto dto, int createdBy)
        {
            var passwordHash = _passwordHasher.HashPassword(dto.Password);
            dto.Password = passwordHash;

            var result = await _userRepository.RegisterTeacher(dto, createdBy);
            return MapUserResponse(result.Rows[0]);
        }

        public async Task<UserResponse> RegisterAdmin(AdminRegistrationDto dto, int createdBy)
        {
            var passwordHash = _passwordHasher.HashPassword(dto.Password);
            dto.Password = passwordHash;

            var result = await _userRepository.RegisterAdmin(dto, createdBy);
            return MapUserResponse(result.Rows[0]);
        }

        public async Task<UserProfileResponse> GetUserProfile(int userId)
        {
            var result = await _userRepository.GetUserProfile(userId);
            return MapUserProfileResponse(result.Rows[0]);
        }

        private UserResponse MapUserResponse(DataRow row)
        {
            return new UserResponse
            {
                Id = Convert.ToInt32(row["id"]),
                Name = row["name"].ToString() ?? string.Empty,
                Email = row["email"].ToString() ?? string.Empty,
                Profile = row["profile_name"].ToString() ?? string.Empty,
                AdditionalInfo = row["department_name"]?.ToString()
                    ?? row["designation"]?.ToString()
                    ?? row["level"]?.ToString()
                    ?? string.Empty
            };
        }

        private UserProfileResponse MapUserProfileResponse(DataRow row)
        {
            return new UserProfileResponse
            {
                Id = Convert.ToInt32(row["id"]),
                Name = row["name"].ToString() ?? string.Empty,
                Email = row["email"].ToString() ?? string.Empty,
                Contact = row["contact"]?.ToString(),
                RoleSpecificInfo = row["reg_no"]?.ToString()
                    ?? row["designation"]?.ToString()
                    ?? row["level"]?.ToString()
                    ?? string.Empty
            };
        }




        public async Task<StudentDetailsResponse> GetStudentByRegNo(int adminId, string regNo)
        {
            return await _userRepository.AdminGetStudentByRegNo(adminId, regNo);
        }

        public async Task<List<TeacherDetailsResponse>> GetTeacherByDetails(int adminId, string name, int depId, string designation)
            {
                return await _userRepository.AdminGetTeacherByDetails(adminId, name, depId, designation);
            }   

        public async Task<bool> UpdateStudent(int adminId, AdminUpdateStudentDto dto)
        {
            var model = new AdminUpdateStudentModel
            {
                AdminId = adminId,
                UserId = dto.UserId,
                Name = dto.Name,
                Email = dto.Email,
                RegNo = dto.RegNo,
                Contact = dto.Contact,
                EnrollYear = dto.EnrollYear,
                CurrentSemester = dto.CurrentSemester,
                AcademicStatus = dto.AcademicStatus,
                Cgpa = dto.Cgpa,
                //ProfilePicUrl = dto.ProfilePicUrl
            };

            var affectedRows = await _userRepository.AdminUpdateStudent(model);
            return affectedRows > 0;
        }

        public async Task<bool> UpdateTeacher(int adminId, AdminUpdateTeacherDto dto)
        {
            var model = new AdminUpdateTeacherModel
            {
                AdminId = adminId,
                UserId = dto.UserId,
                Name = dto.Name,
                Email = dto.Email,
                Contact = dto.Contact,
                Designation = dto.Designation,
                Qualification = dto.Qualification,
                Specialization = dto.Specialization,
                ExperienceYears = dto.ExperienceYears,
                ProfilePicUrl = dto.ProfilePicUrl
            };

            var affectedRows = await _userRepository.AdminUpdateTeacher(model);
            return affectedRows > 0;
        }

        public async Task<(bool Success, string Message)> SoftDeleteUser(int adminId, int userId)
        {
            return await _userRepository.AdminSoftDeleteUser(adminId, userId);
        }

        public async Task<int> CountActiveUsers(int adminId, string profileName)
        {
            return await _userRepository.AdminCountActiveUsers(adminId, profileName);
        }

        public async Task<List<UserResponse>> GetAllUsersByProfile(int adminId, AdminGetAllUsersDto dto)
        {
            return await _userRepository.AdminGetAllUsersByProfile(
                adminId,
                dto.ProfileName,
                dto.Limit,
                dto.Offset);
        }
    }
}

