using backend.Models;

namespace backend.Repositories
{
    public interface ISemesterRepository
    {
        Task<Semester> Create(Semester semester, int userId);
        Task<Semester?> GetById(int id);
        Task<IEnumerable<Semester>> GetAll();
        Task<Semester?> GetCurrent();
        Task<Semester> Update(Semester semester, int userId);
        Task<bool> Delete(int id, int userId);
    }
}
