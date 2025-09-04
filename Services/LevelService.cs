// Services/Implementations/LevelService.cs
using backend.Dtos;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class LevelService : ILevelService
    {
        private readonly ILevelRepository _repository;

        public LevelService(ILevelRepository repository)
        {
            _repository = repository;
        }

        public async Task<LevelGetDto> Create(LevelCreateDto dto, int userId)
        {
            var level = new Level
            {
                Name = dto.Name,
                Description = dto.Description
            };

            var createdLevel = await _repository.Create(level, userId);
            return MapToDto(createdLevel);
        }

        public async Task<LevelGetDto?> GetById(int id)
        {
            var level = await _repository.GetById(id);
            return level == null ? null : MapToDto(level);
        }

        public async Task<IEnumerable<LevelGetDto>> GetAll()
        {
            var levels = await _repository.GetAll();
            return levels.Select(MapToDto);
        }

        public async Task<LevelGetDto?> Update(int id, LevelUpdateDto dto, int userId)
        {
            var level = new Level
            {
                Id = id,
                Name = dto.Name,
                Description = dto.Description
            };

            var updatedLevel = await _repository.Update(level, userId);
            return updatedLevel == null ? null : MapToDto(updatedLevel);
        }

        public async Task<bool> Delete(int id, int userId)
        {
            return await _repository.Delete(id, userId);
        }

        private static LevelGetDto MapToDto(Level level)
        {
            return new LevelGetDto
            {
                Id = level.Id,
                Name = level.Name,
                Description = level.Description,
                CreatedAt = level.CreatedAt,
                IsActive = level.IsActive
            };
        }
    }
}