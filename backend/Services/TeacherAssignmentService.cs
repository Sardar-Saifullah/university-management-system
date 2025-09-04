

// Services/Implementations/TeacherAssignmentService.cs
using backend.Dtos;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class TeacherAssignmentService : ITeacherAssignmentService
    {
        private readonly ITeacherAssignmentRepository _assignmentRepository;
        private readonly IUserManagementRepository _userRepository;

        public TeacherAssignmentService(ITeacherAssignmentRepository assignmentRepository, IUserManagementRepository userRepository)
        {
            _assignmentRepository = assignmentRepository;
            _userRepository = userRepository;
        }

        public async Task<TeacherAssignmentResponseDto> AssignTeacherToCourseAsync(TeacherAssignmentCreateDto dto, int adminId)
        {
            var assignmentId = await _assignmentRepository.AssignTeacherToCourseAsync(
                dto.TeacherId, dto.CourseOfferingId, dto.IsPrimary, adminId);

            return await GetAssignmentByIdAsync(assignmentId);
        }

        public async Task<TeacherAssignmentResponseDto> UpdateTeacherAssignmentAsync(int assignmentId, TeacherAssignmentUpdateDto dto, int adminId)
        {
            var success = await _assignmentRepository.UpdateTeacherAssignmentAsync(assignmentId, dto.IsPrimary, adminId);

            if (!success)
                throw new Exception("Failed to update teacher assignment");

            return await GetAssignmentByIdAsync(assignmentId);
        }

        public async Task<bool> RemoveTeacherAssignmentAsync(int assignmentId, int adminId)
        {
            return await _assignmentRepository.RemoveTeacherAssignmentAsync(assignmentId, adminId);
        }

        public async Task<IEnumerable<TeacherAssignmentResponseDto>> GetAllAssignmentsAsync(int adminId, bool? filterIsActive)
        {
            var assignments = await _assignmentRepository.GetAllAssignmentsAsync(adminId, filterIsActive);
            return assignments.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<TeacherAssignmentResponseDto>> GetTeacherAssignmentsAsync(int userId, bool includeInactive)
        {
            var assignments = await _assignmentRepository.GetTeacherAssignmentsAsync(userId, includeInactive);
            return assignments.Select(MapToResponseDto);
        }

        public async Task<TeacherAssignmentResponseDto> GetAssignmentByIdAsync(int id)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(id);
            if (assignment == null)
                throw new Exception("Assignment not found");

            // Get detailed information
            var detailedAssignments = await _assignmentRepository.GetAllAssignmentsAsync(assignment.CreatedBy ?? 0, true);
            var detailed = detailedAssignments.FirstOrDefault(a => a.Id == id);

            if (detailed == null)
                throw new Exception("Assignment details not found");

            return MapToResponseDto(detailed);
        }

        private TeacherAssignmentResponseDto MapToResponseDto(TeacherAssignmentDetail assignment)
        {
            return new TeacherAssignmentResponseDto
            {
                Id = assignment.Id,
                TeacherId = assignment.TeacherId,
                TeacherName = assignment.TeacherName,
                CourseOfferingId = assignment.CourseOfferingId,
                CourseCode = assignment.CourseCode,
                CourseTitle = assignment.CourseTitle,
                SemesterName = assignment.SemesterName,
                IsPrimary = assignment.IsPrimary,
                CreatedAt = assignment.CreatedAt,
                ModifiedAt = assignment.ModifiedAt,
                IsActive = assignment.IsActive
            };
        }
    }
}