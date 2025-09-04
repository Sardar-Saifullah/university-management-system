// Repositories/ICreditLimitOverrideRepository.cs
using backend.Models;

namespace backend.Repositories
{
    public interface ICreditLimitOverrideRepository
    {
        Task<int> CreateAsync(CreditLimitOverride overrideEntity, int approvedByUserId);
        Task<bool> UpdateAsync(CreditLimitOverride overrideEntity, int modifiedBy);
        Task<IEnumerable<CreditLimitOverride>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<CreditLimitOverride>> GetAllAsync(bool? activeOnly = true, int? departmentId = null, int? programId = null);
        Task<CreditLimitOverride?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id, int modifiedBy);
        Task<CreditLimitOverride?> GetActiveOverrideForStudentAsync(int studentId);
    }
}