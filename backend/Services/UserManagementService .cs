using backend.Dtos;
using backend.Repositories;
using System.Security.Claims;

namespace backend.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserManagementRepository _repository;
        private readonly IPermissionService _permissionService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserManagementService(
            IUserManagementRepository repository,
            IPermissionService permissionService,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _permissionService = permissionService;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetCurrentUserProfile()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue("profile") ?? string.Empty;
        }

        private async Task<bool> CheckPermissionAsync(string activityName, string action)
        {
            var profileName = GetCurrentUserProfile();
            if (string.IsNullOrEmpty(profileName))
                return false;

            // For simplicity, we'll check if the profile has permission for the base activity
            // You might need to adjust this based on your actual permission structure
            return await _permissionService.CheckPermission(profileName, activityName);
        }

        public async Task<PaginationResponseDto<UserResponseDto>> GetUsersAsync(int adminId, PaginationRequestDto request, int? profileId = null, bool? isActive = null)
        {
            if (!await CheckPermissionAsync("users", "read"))
                throw new UnauthorizedAccessException("Permission denied for reading users");

            return await _repository.GetUsersAsync(request, profileId, isActive);
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(int adminId, int userId)
        {
            if (!await CheckPermissionAsync("users", "read"))
                throw new UnauthorizedAccessException("Permission denied for reading users");

            return await _repository.GetUserByIdAsync(userId);
        }

        public async Task<bool> UpdateUserAsync(int adminId, int userId, UserUpdateDto updateDto)
        {
            if (!await CheckPermissionAsync("users", "update"))
                throw new UnauthorizedAccessException("Permission denied for updating users");

            var affectedRows = await _repository.UpdateUserAsync(adminId, userId, updateDto);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteUserAsync(int adminId, int userId)
        {
            if (!await CheckPermissionAsync("users", "delete"))
                throw new UnauthorizedAccessException("Permission denied for deleting users");

            var affectedRows = await _repository.DeleteUserAsync(adminId, userId);
            return affectedRows > 0;
        }

        public async Task<PaginationResponseDto<StudentProfileResponseDto>> GetStudentProfilesAsync(int adminId, PaginationRequestDto request, string? academicStatus = null)
        {
            if (!await CheckPermissionAsync("student_profiles", "read"))
                throw new UnauthorizedAccessException("Permission denied for reading student profiles");

            return await _repository.GetStudentProfilesAsync(request, academicStatus);
        }

        public async Task<StudentProfileResponseDto?> GetStudentProfileByIdAsync(int adminId, int studentId)
        {
            if (!await CheckPermissionAsync("student_profiles", "read"))
                throw new UnauthorizedAccessException("Permission denied for reading student profiles");

            return await _repository.GetStudentProfileByIdAsync(studentId);
        }

        public async Task<bool> UpdateStudentProfileAsync(int adminId, int studentId, StudentProfileUpdateDto updateDto)
        {
            if (!await CheckPermissionAsync("student_profiles", "update"))
                throw new UnauthorizedAccessException("Permission denied for updating student profiles");

            var affectedRows = await _repository.UpdateStudentProfileAsync(adminId, studentId, updateDto);
            return affectedRows > 0;
        }

        public async Task<PaginationResponseDto<TeacherProfileResponseDto>> GetTeacherProfilesAsync(int adminId, PaginationRequestDto request, int? depId = null)
        {
            if (!await CheckPermissionAsync("teacher_profiles", "read"))
                throw new UnauthorizedAccessException("Permission denied for reading teacher profiles");

            return await _repository.GetTeacherProfilesAsync(request, depId);
        }

        public async Task<TeacherProfileResponseDto?> GetTeacherProfileByIdAsync(int adminId, int teacherId)
        {
            if (!await CheckPermissionAsync("teacher_profiles", "read"))
                throw new UnauthorizedAccessException("Permission denied for reading teacher profiles");

            return await _repository.GetTeacherProfileByIdAsync(teacherId);
        }

        public async Task<bool> UpdateTeacherProfileAsync(int adminId, int teacherId, TeacherProfileUpdateDto updateDto)
        {
            if (!await CheckPermissionAsync("teacher_profiles", "update"))
                throw new UnauthorizedAccessException("Permission denied for updating teacher profiles");

            var affectedRows = await _repository.UpdateTeacherProfileAsync(adminId, teacherId, updateDto);
            return affectedRows > 0;
        }

        public async Task<PaginationResponseDto<AdminProfileResponseDto>> GetAdminProfilesAsync(int adminId, PaginationRequestDto request, string? level = null)
        {
            if (!await CheckPermissionAsync("admin_profiles", "read"))
                throw new UnauthorizedAccessException("Permission denied for reading admin profiles");

            return await _repository.GetAdminProfilesAsync(request, level);
        }

        public async Task<AdminProfileResponseDto?> GetAdminProfileByIdAsync(int adminId, int adminProfileId)
        {
            if (!await CheckPermissionAsync("admin_profiles", "read"))
                throw new UnauthorizedAccessException("Permission denied for reading admin profiles");

            return await _repository.GetAdminProfileByIdAsync(adminProfileId);
        }

        public async Task<bool> UpdateAdminProfileAsync(int adminId, int targetAdminId, AdminProfileUpdateDto updateDto)
        {
            if (!await CheckPermissionAsync("admin_profiles", "update"))
                throw new UnauthorizedAccessException("Permission denied for updating admin profiles");

            var affectedRows = await _repository.UpdateAdminProfileAsync(adminId, targetAdminId, updateDto);
            return affectedRows > 0;
        }
    }
}