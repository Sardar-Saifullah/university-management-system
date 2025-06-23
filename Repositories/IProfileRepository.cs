using backend.Dtos;
using backend.Models;
using System.Data;
using Microsoft.AspNetCore.Http;

namespace backend.Repositories
{
    public interface IProfileRepository
    {
        Task<AdminProfileResponse> GetAdminProfileAsync(int adminId);
        Task<bool> UpdateAdminProfileAsync(int adminId, AdminProfileUpdateDto dto, IFormFile? profilePicFile = null);
        Task<bool> DeleteAdminProfilePicAsync(int adminId);

        Task<TeacherProfileResponse> GetTeacherProfileAsync(int teacherId);
        Task<bool> UpdateTeacherProfileAsync(int teacherId, TeacherProfileUpdateDto dto, IFormFile? profilePicFile = null);
        Task<bool> UpdateTeacherQualificationsAsync(int teacherId, TeacherQualificationsUpdateDto dto);
        Task<bool> DeleteTeacherProfilePicAsync(int teacherId);

        Task<StudentProfileResponse> GetStudentProfileAsync(int studentId);
        Task<bool> UpdateStudentProfileAsync(int studentId, StudentProfileUpdateDto dto, IFormFile? profilePicFile = null);
        Task<bool> DeleteStudentProfilePicAsync(int studentId);
    }
}