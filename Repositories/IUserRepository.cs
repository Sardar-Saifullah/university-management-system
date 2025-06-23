using backend.Dtos;
using backend.Models;
using System.Data;
namespace backend.Repositories
{
    public interface IUserRepository
    {
        Task<DataTable> RegisterStudent(StudentRegistrationDto dto, int createdBy);
        Task<DataTable> RegisterTeacher(TeacherRegistrationDto dto, int createdBy);
        Task<DataTable> RegisterAdmin(AdminRegistrationDto dto, int createdBy);
        Task<DataTable> GetUserProfile(int userId);


        Task<StudentDetailsResponse> AdminGetStudentByRegNo(int adminId, string regNo);
        Task<List<TeacherDetailsResponse>> AdminGetTeacherByDetails(int adminId, string name, int depId, string designation);
        Task<int> AdminUpdateStudent(AdminUpdateStudentModel model);
        Task<int> AdminUpdateTeacher(AdminUpdateTeacherModel model);
        Task<(bool Success, string Message)> AdminSoftDeleteUser(int adminId, int userId);
        Task<int> AdminCountActiveUsers(int adminId, string profileName);
        Task<List<UserResponse>> AdminGetAllUsersByProfile(int adminId, string profileName, int limit, int offset);

    }
}
