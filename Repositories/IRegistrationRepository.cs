using backend.Models;

namespace backend.Repositories
{
    public interface IRegistrationRepository
    {
        Task<StudentProfile> RegisterStudent(User user, StudentProfile profile, int createdBy);
        Task<TeacherProfile> RegisterTeacher(User user, TeacherProfile profile, int createdBy);
        Task<AdminProfile> RegisterAdmin(User user, AdminProfile profile, int createdBy);
    }
}