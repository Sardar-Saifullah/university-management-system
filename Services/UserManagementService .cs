using backend.Dtos;
using backend.Repositories;
using MySql.Data.MySqlClient;

namespace backend.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserManagementRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserManagementService(
            IUserManagementRepository repository,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PaginationResponseDto<UserResponseDto>> GetUsersAsync(int adminId, PaginationRequestDto request, int? profileId = null, bool? isActive = null)
        {
            return await _repository.GetUsersAsync(request, profileId, isActive);
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(int adminId, int userId)
        {
            return await _repository.GetUserByIdAsync(userId);
        }

        public async Task<bool> UpdateUserAsync(int adminId, int userId, UserUpdateDto updateDto)
        {
            // First verify the user exists
            var existingUser = await _repository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                return false;
            }

            // Try to update - even if no changes are made, we consider it successful
            // as long as the user exists
            try
            {
                var affectedRows = await _repository.UpdateUserAsync(adminId, userId, updateDto);
                return true; // User exists, so consider it successful regardless of changes
            }
            catch
            {
                return false; // Some other error occurred
            }

        }

        public async Task<bool> DeleteUserAsync(int adminId, int userId)
        {
            try
            {
                var affectedRows = await _repository.DeleteUserAsync(adminId, userId);

                // Return true if rows were affected OR if the user doesn't exist (already deleted)
                // The desired state is "user deleted", which is already achieved if they don't exist
                return true;
            }
            catch (Exception ex) when (ex.Message.Contains("User not found") ||
                                      ex.Message.Contains("not found"))
            {
                // User doesn't exist, which means the desired state is already achieved
                return true;
            }
            catch
            {
                // Some other error occurred during deletion
                return false;
            }
        }

        public async Task<PaginationResponseDto<StudentProfileResponseDto>> GetStudentProfilesAsync(int adminId, PaginationRequestDto request, string? academicStatus = null)
        {
            return await _repository.GetStudentProfilesAsync(request, academicStatus);
        }

        public async Task<StudentProfileResponseDto?> GetStudentProfileByIdAsync(int adminId, int studentId)
        {
            return await _repository.GetStudentProfileByIdAsync(studentId);
        }

        public async Task<bool> UpdateStudentProfileAsync(int adminId, int studentId, StudentProfileUpdateDto updateDto)
        {
            // First check if student exists
            var existingStudent = await _repository.GetStudentProfileByIdAsync(studentId);
            if (existingStudent == null)
            {
                return false;
            }

            // Perform the update - consider it successful as long as student exists
            try
            {
                var affectedRows = await _repository.UpdateStudentProfileAsync(adminId, studentId, updateDto);
                return true; // Student exists, so consider it successful regardless of changes
            }
            catch (Exception ex) when (ex.Message.Contains("not found") || ex.Message.Contains("Student not found"))
            {
                return false;
            }
            catch
            {
                return false; // Some other error occurred
            }
        }

        public async Task<PaginationResponseDto<TeacherProfileResponseDto>> GetTeacherProfilesAsync(int adminId, PaginationRequestDto request, int? depId = null)
        {
            return await _repository.GetTeacherProfilesAsync(request, depId);
        }

        public async Task<TeacherProfileResponseDto?> GetTeacherProfileByIdAsync(int adminId, int teacherId)
        {
            return await _repository.GetTeacherProfileByIdAsync(teacherId);
        }

        public async Task<bool> UpdateTeacherProfileAsync(int adminId, int teacherId, TeacherProfileUpdateDto updateDto)
        {
            // First check if teacher exists
            var existingTeacher = await _repository.GetTeacherProfileByIdAsync(teacherId);
            if (existingTeacher == null)
            {
                return false;
            }

            try
            {
                var affectedRows = await _repository.UpdateTeacherProfileAsync(adminId, teacherId, updateDto);

                // Return true if teacher exists, regardless of whether changes were made
                // This is acceptable because the desired state (teacher exists with current data) is achieved
                return true;
            }
            catch (Exception ex) when (ex.Message.Contains("not found") || ex.Message.Contains("Teacher not found"))
            {
                return false;
            }
            catch (Exception ex)
            {
                // Log the actual error for debugging
                Console.WriteLine($"Error updating teacher: {ex.Message}");
                return false;
            }
        }

        public async Task<PaginationResponseDto<AdminProfileResponseDto>> GetAdminProfilesAsync(int adminId, PaginationRequestDto request, string? level = null)
        {
            return await _repository.GetAdminProfilesAsync(request, level);
        }

        public async Task<AdminProfileResponseDto?> GetAdminProfileByIdAsync(int adminId, int adminProfileId)
        {
            return await _repository.GetAdminProfileByIdAsync(adminProfileId);
        }

        public async Task<bool> UpdateAdminProfileAsync(int adminId, int targetAdminId, AdminProfileUpdateDto updateDto)
        {
            try
            {
                var existingAdmin = await _repository.GetAdminProfileByIdAsync(targetAdminId);
                if (existingAdmin == null)
                {
                    return false;
                }

                var affectedRows = await _repository.UpdateAdminProfileAsync(adminId, targetAdminId, updateDto);

                // Log the update attempt
                Console.WriteLine($"Admin {adminId} updated admin {targetAdminId}. Rows affected: {affectedRows}");

                return affectedRows >= 0; // Return true even if no rows changed (0) or if rows were updated (>0)
            }
            catch (MySqlException mysqlEx)
            {
                Console.WriteLine($"MySQL Error updating admin: {mysqlEx.Message}");
                Console.WriteLine($"Error Number: {mysqlEx.Number}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating admin: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return false;
            }
        }
    }
}