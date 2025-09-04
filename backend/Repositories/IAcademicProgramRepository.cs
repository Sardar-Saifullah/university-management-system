using backend.Models;
namespace backend.Repositories
{
    public interface IAcademicProgramRepository
    {
        Task<AcademicProgram> CreateAsync(AcademicProgram program);
        Task<AcademicProgram> GetByIdAsync(int id);
        Task<IEnumerable<AcademicProgram>> GetAllAsync();
        Task<bool> UpdateAsync(AcademicProgram program);
        Task<bool> DeleteAsync(int id, int deletedBy);
    }
}
