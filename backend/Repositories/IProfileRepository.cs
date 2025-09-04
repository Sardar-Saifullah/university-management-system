using backend.Dtos;

namespace backend.Repositories
{
    public interface IProfileRepository
    {
        Task<AdminProfileDto> GetAdminProfile(int adminId);
        Task<bool> UpdateAdminProfile(int adminId, AdminOwnProfileUpdateDto updateDto);

        Task<TeacherProfileDto> GetTeacherProfile(int teacherId);
        Task<bool> UpdateTeacherProfile(int teacherId, TeacherOwnProfileUpdateDto updateDto);
        Task<bool> UpdateTeacherQualifications(int teacherId, TeacherQualificationUpdateDto updateDto);

        Task<StudentProfileDto> GetStudentProfile(int studentId);
        Task<bool> UpdateStudentProfile(int studentId, StudentOwnProfileUpdateDto updateDto);
    }
}