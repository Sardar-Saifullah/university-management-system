using backend.Dtos;

namespace backend.Services
{
 
        public interface ITeacherAssignmentService
        {
            Task<TeacherAssignmentResponseDto> AssignTeacherToCourseAsync(TeacherAssignmentCreateDto dto, int adminId);
            Task<TeacherAssignmentResponseDto> UpdateTeacherAssignmentAsync(int assignmentId, TeacherAssignmentUpdateDto dto, int adminId);
            Task<bool> RemoveTeacherAssignmentAsync(int assignmentId, int adminId);
            Task<IEnumerable<TeacherAssignmentResponseDto>> GetAllAssignmentsAsync(int adminId, bool? filterIsActive);
            Task<IEnumerable<TeacherAssignmentResponseDto>> GetTeacherAssignmentsAsync(int userId, bool includeInactive);
            Task<TeacherAssignmentResponseDto> GetAssignmentByIdAsync(int id);
        }
}
