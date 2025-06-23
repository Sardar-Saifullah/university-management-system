using backend.Dtos;
using backend.Models;
using System.Data;
namespace backend.Services
{
    public interface IUserService
    {
        Task<UserResponse> RegisterStudent(StudentRegistrationDto dto, int createdBy);
        Task<UserResponse> RegisterTeacher(TeacherRegistrationDto dto, int createdBy);
        Task<UserResponse> RegisterAdmin(AdminRegistrationDto dto, int createdBy);
        Task<UserProfileResponse> GetUserProfile(int userId);

        Task<StudentDetailsResponse> GetStudentByRegNo(int adminId, string regNo);
        Task<List<TeacherDetailsResponse>> GetTeacherByDetails(int adminId, string name, int depId, string designation);
        Task<bool> UpdateStudent(int adminId, AdminUpdateStudentDto dto);
        Task<bool> UpdateTeacher(int adminId, AdminUpdateTeacherDto dto);
        Task<(bool Success, string Message)> SoftDeleteUser(int adminId, int userId);
        Task<int> CountActiveUsers(int adminId, string profileName);
        Task <List<UserResponse>> GetAllUsersByProfile(int adminId, AdminGetAllUsersDto dto);
    }
}
