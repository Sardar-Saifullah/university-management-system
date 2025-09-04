// Services/ProfileService.cs
using backend.Dtos;
using backend.Repositories;

namespace backend.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;

        public ProfileService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<AdminProfileDto> GetAdminProfile(int adminId)
        {
            return await _profileRepository.GetAdminProfile(adminId);
        }

        public async Task<bool> UpdateAdminProfile(int adminId, AdminOwnProfileUpdateDto updateDto)
        {
            return await _profileRepository.UpdateAdminProfile(adminId, updateDto);
        }

        public async Task<TeacherProfileDto> GetTeacherProfile(int teacherId)
        {
            return await _profileRepository.GetTeacherProfile(teacherId);
        }

        public async Task<bool> UpdateTeacherProfile(int teacherId, TeacherOwnProfileUpdateDto updateDto)
        {
            return await _profileRepository.UpdateTeacherProfile(teacherId, updateDto);
        }

        public async Task<bool> UpdateTeacherQualifications(int teacherId, TeacherQualificationUpdateDto updateDto)
        {
            return await _profileRepository.UpdateTeacherQualifications(teacherId, updateDto);
        }

        public async Task<StudentProfileDto> GetStudentProfile(int studentId)
        {
            return await _profileRepository.GetStudentProfile(studentId);
        }

        public async Task<bool> UpdateStudentProfile(int studentId, StudentOwnProfileUpdateDto updateDto)
        {
            return await _profileRepository.UpdateStudentProfile(studentId, updateDto);
        }
    }
}