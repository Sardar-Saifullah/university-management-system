using backend.Models;

namespace backend.Repositories
{
    public interface ITeacherAssignmentRepository
    {
        Task<int> AssignTeacherToCourseAsync(int teacherId, int courseOfferingId, bool isPrimary, int adminId);
        Task<bool> UpdateTeacherAssignmentAsync(int assignmentId, bool isPrimary, int adminId);
        Task<bool> RemoveTeacherAssignmentAsync(int assignmentId, int adminId);
        Task<IEnumerable<TeacherAssignmentDetail>> GetAllAssignmentsAsync(int adminId, bool? filterIsActive);
        Task<IEnumerable<TeacherAssignmentDetail>> GetTeacherAssignmentsAsync(int userId, bool includeInactive);
        Task<TeacherAssignment> GetByIdAsync(int id);
    }
}
