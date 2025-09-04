using backend.Dtos;

namespace backend.Services
{
    public interface IRegistrationService
    {
        Task<StudentRegistrationResponse> RegisterStudent(StudentRegistrationDto request, int createdBy);
        Task<TeacherRegistrationResponse> RegisterTeacher(TeacherRegistrationDto request, int createdBy);
        Task<AdminRegistrationResponse> RegisterAdmin(AdminRegistrationDto request, int createdBy);
    }
}