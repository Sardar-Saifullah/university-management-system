using backend.Dtos;

namespace backend.Services
{
    public interface IUserManagementService
    {
        Task<PaginationResponseDto<UserResponseDto>> GetUsersAsync(int adminId, PaginationRequestDto request, int? profileId = null, bool? isActive = null);
        Task<UserResponseDto?> GetUserByIdAsync(int adminId, int userId);
        Task<bool> UpdateUserAsync(int adminId, int userId, UserUpdateDto updateDto);
        Task<bool> DeleteUserAsync(int adminId, int userId);

        Task<PaginationResponseDto<StudentProfileResponseDto>> GetStudentProfilesAsync(int adminId, PaginationRequestDto request, string? academicStatus = null);
        Task<StudentProfileResponseDto?> GetStudentProfileByIdAsync(int adminId, int studentId);
        Task<bool> UpdateStudentProfileAsync(int adminId, int studentId, StudentProfileUpdateDto updateDto);

        Task<PaginationResponseDto<TeacherProfileResponseDto>> GetTeacherProfilesAsync(int adminId, PaginationRequestDto request, int? depId = null);
        Task<TeacherProfileResponseDto?> GetTeacherProfileByIdAsync(int adminId, int teacherId);
        Task<bool> UpdateTeacherProfileAsync(int adminId, int teacherId, TeacherProfileUpdateDto updateDto);

        Task<PaginationResponseDto<AdminProfileResponseDto>> GetAdminProfilesAsync(int adminId, PaginationRequestDto request, string? level = null);
        Task<AdminProfileResponseDto?> GetAdminProfileByIdAsync(int adminId, int adminProfileId);
        Task<bool> UpdateAdminProfileAsync(int adminId, int targetAdminId, AdminProfileUpdateDto updateDto);
    }
}