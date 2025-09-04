using backend.Dtos;
using backend.Models;

namespace backend.Repositories
{
    public interface IUserManagementRepository
    {
        Task<PaginationResponseDto<UserResponseDto>> GetUsersAsync(PaginationRequestDto request, int? profileId = null, bool? isActive = null);
        Task<UserResponseDto?> GetUserByIdAsync(int userId);
        Task<int> UpdateUserAsync(int adminId, int userId, UserUpdateDto updateDto);
        Task<int> DeleteUserAsync(int adminId, int userId);

        Task<PaginationResponseDto<StudentProfileResponseDto>> GetStudentProfilesAsync(PaginationRequestDto request, string? academicStatus = null);
        Task<StudentProfileResponseDto?> GetStudentProfileByIdAsync(int studentId);
        Task<int> UpdateStudentProfileAsync(int adminId, int studentId, StudentProfileUpdateDto updateDto);

        Task<PaginationResponseDto<TeacherProfileResponseDto>> GetTeacherProfilesAsync(PaginationRequestDto request, int? depId = null);
        Task<TeacherProfileResponseDto?> GetTeacherProfileByIdAsync(int teacherId);
        Task<int> UpdateTeacherProfileAsync(int adminId, int teacherId, TeacherProfileUpdateDto updateDto);

        Task<PaginationResponseDto<AdminProfileResponseDto>> GetAdminProfilesAsync(PaginationRequestDto request, string? level = null);
        Task<AdminProfileResponseDto?> GetAdminProfileByIdAsync(int adminProfileId);
        Task<int> UpdateAdminProfileAsync(int adminId, int targetAdminId, AdminProfileUpdateDto updateDto);
    }
}