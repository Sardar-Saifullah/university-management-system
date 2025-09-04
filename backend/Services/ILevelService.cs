// Services/Interfaces/ILevelService.cs
using backend.Dtos;

namespace backend.Services
{
    public interface ILevelService
    {
        Task<LevelGetDto> Create(LevelCreateDto dto, int userId);
        Task<LevelGetDto?> GetById(int id);
        Task<IEnumerable<LevelGetDto>> GetAll();
        Task<LevelGetDto?> Update(int id, LevelUpdateDto dto, int userId);
        Task<bool> Delete(int id, int userId);
    }
}