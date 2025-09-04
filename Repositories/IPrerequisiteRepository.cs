// Repositories/Interfaces/IPrerequisiteRepository.cs
using backend.Dtos;
using backend.Models;

namespace backend.Repositories
{
    public interface IPrerequisiteRepository
    {
        Task AddPrerequisite(CoursePrerequisite prerequisite, int userId);
        Task RemovePrerequisite(int prerequisiteId, int userId);
        Task<List<PrerequisiteDto>> GetPrerequisites(int courseId, bool includeInactive, int userId);
        Task<PrerequisiteValidationResult> ValidatePrerequisites(int studentId, int courseId);
    }
}

