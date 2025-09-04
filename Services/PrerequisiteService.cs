// Services/PrerequisiteService.cs
using backend.Dtos;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class PrerequisiteService : IPrerequisiteService
    {
        private readonly IPrerequisiteRepository _prerequisiteRepository;

        public PrerequisiteService(IPrerequisiteRepository prerequisiteRepository)
        {
            _prerequisiteRepository = prerequisiteRepository;
        }

        public async Task AddPrerequisite(AddPrerequisiteDto dto, int userId)
        {
            var prerequisite = new CoursePrerequisite
            {
                CourseId = dto.CourseId,
                PrerequisiteCourseId = dto.PrerequisiteCourseId,
                IsMandatory = dto.IsMandatory,
                MinimumGrade = dto.MinimumGrade
            };

            await _prerequisiteRepository.AddPrerequisite(prerequisite, userId);
        }

        public async Task RemovePrerequisite(RemovePrerequisiteDto dto, int userId)
        {
            await _prerequisiteRepository.RemovePrerequisite(dto.PrerequisiteId, userId);
        }

        public async Task<List<PrerequisiteDto>> GetPrerequisites(int courseId, bool includeInactive, int userId)
        {
            return await _prerequisiteRepository.GetPrerequisites(courseId, includeInactive, userId);
        }

        public async Task<PrerequisiteValidationResult> ValidatePrerequisites(ValidatePrerequisitesDto dto)
        {
            return await _prerequisiteRepository.ValidatePrerequisites(dto.StudentId, dto.CourseId);
        }
    }
}