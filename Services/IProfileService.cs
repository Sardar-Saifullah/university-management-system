using backend.Dtos;
using Microsoft.AspNetCore.Http;
namespace backend.Services
{
    public interface IProfileService
    {
        Task<AdminProfileResponse> GetAdminProfileAsync(int adminId);
        Task<AdminProfileResponse> UpdateAdminProfileAsync(int adminId, AdminProfileUpdateDto dto, IFormFile? profilePicFile = null);
        Task<bool> DeleteAdminProfilePicAsync(int adminId);

        Task<TeacherProfileResponse> GetTeacherProfileAsync(int teacherId);
        Task<TeacherProfileResponse> UpdateTeacherProfileAsync(int teacherId, TeacherProfileUpdateDto dto, IFormFile? profilePicFile = null);
        Task<TeacherProfileResponse> UpdateTeacherQualificationsAsync(int teacherId, TeacherQualificationsUpdateDto dto);
        Task<bool> DeleteTeacherProfilePicAsync(int teacherId);

        Task<StudentProfileResponse> GetStudentProfileAsync(int studentId);
        Task<StudentProfileResponse> UpdateStudentProfileAsync(int studentId, StudentProfileUpdateDto dto, IFormFile? profilePicFile = null);
        Task<bool> DeleteStudentProfilePicAsync(int studentId);
    }
}