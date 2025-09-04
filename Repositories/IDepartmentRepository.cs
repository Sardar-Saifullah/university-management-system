using backend.Models;

namespace backend.Repositories
{
   
        public interface IDepartmentRepository
        {
            Task<Department> Create(Department department);
            Task<Department> GetById(int id);
            Task<IEnumerable<Department>> GetAll();
            Task<Department> Update(Department department);
            Task<bool> Delete(int id, int deletedBy);
        }
    
}
