using backend.Dtos;
using backend.Repositories;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace backend.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;

        public ProfileService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<AdminProfileResponse> GetAdminProfileAsync(int adminId)
        {
            return await _profileRepository.GetAdminProfileAsync(adminId);
        }

        public async Task<AdminProfileResponse> UpdateAdminProfileAsync(int adminId, AdminProfileUpdateDto dto, IFormFile? profilePicFile = null)
        {
            var success = await _profileRepository.UpdateAdminProfileAsync(adminId, dto, profilePicFile);
            if (!success)
            {
                throw new InvalidOperationException("Failed to update admin profile");
            }
            return await GetAdminProfileAsync(adminId);
        }

        public async Task<bool> DeleteAdminProfilePicAsync(int adminId)
        {
            return await _profileRepository.DeleteAdminProfilePicAsync(adminId);
        }

        public async Task<TeacherProfileResponse> GetTeacherProfileAsync(int teacherId)
        {
            return await _profileRepository.GetTeacherProfileAsync(teacherId);
        }

        public async Task<TeacherProfileResponse> UpdateTeacherProfileAsync(int teacherId, TeacherProfileUpdateDto dto, IFormFile? profilePicFile = null)
        {
            var success = await _profileRepository.UpdateTeacherProfileAsync(teacherId, dto, profilePicFile);
            if (!success)
            {
                throw new InvalidOperationException("Failed to update teacher profile");
            }

            return await GetTeacherProfileAsync(teacherId);
        }
        public async Task<TeacherProfileResponse> UpdateTeacherQualificationsAsync(int teacherId, TeacherQualificationsUpdateDto dto)
        {
            var success = await _profileRepository.UpdateTeacherQualificationsAsync(teacherId, dto);
            if (!success)
            {
                throw new InvalidOperationException("Failed to update teacher profile");
            }

            return await GetTeacherProfileAsync(teacherId);
        }

        public async Task<bool> DeleteTeacherProfilePicAsync(int teacherId)
        {
           return await _profileRepository.DeleteTeacherProfilePicAsync(teacherId);
            
        }

        public async Task<StudentProfileResponse> GetStudentProfileAsync(int studentId)
        {
            return await _profileRepository.GetStudentProfileAsync(studentId);
        }

        public async Task<StudentProfileResponse> UpdateStudentProfileAsync(int studentId, StudentProfileUpdateDto dto, IFormFile? profilePicFile = null)
        {
            var success = await _profileRepository.UpdateStudentProfileAsync(studentId, dto, profilePicFile);
            if (!success)
            {
                throw new InvalidOperationException("Failed to update student profile");
            }

            return await GetStudentProfileAsync(studentId);
        }

        public async Task<bool> DeleteStudentProfilePicAsync(int studentId)
        {
           return await _profileRepository.DeleteStudentProfilePicAsync(studentId);
          
        }
    }
}