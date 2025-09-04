// Repositories/Interfaces/ILevelRepository.cs
using backend.Models;

namespace backend.Repositories
{
    public interface ILevelRepository
    {
        Task<Level> Create(Level level, int userId);
        Task<Level?> GetById(int id);
        Task<IEnumerable<Level>> GetAll();
        Task<Level?> Update(Level level, int userId);
        Task<bool> Delete(int id, int userId);
    }
}