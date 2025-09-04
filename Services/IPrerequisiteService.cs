// Services/Interfaces/IPrerequisiteService.cs
using backend.Dtos;
using backend.Models;

namespace backend.Services
{
    public interface IPrerequisiteService
    {
        Task AddPrerequisite(AddPrerequisiteDto dto, int userId);
        Task RemovePrerequisite(RemovePrerequisiteDto dto, int userId);
        Task<List<PrerequisiteDto>> GetPrerequisites(int courseId, bool includeInactive, int userId);
        Task<PrerequisiteValidationResult> ValidatePrerequisites(ValidatePrerequisitesDto dto);
    }
}