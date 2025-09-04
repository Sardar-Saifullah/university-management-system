// Repositories/ICreditLimitPolicyRepository.cs
using backend.Models;

namespace backend.Repositories
{
    public interface ICreditLimitPolicyRepository
    {
        Task<int> CreateAsync(CreditLimitPolicy policy, int createdBy);
        Task<bool> UpdateAsync(CreditLimitPolicy policy, int modifiedBy);
        Task<IEnumerable<CreditLimitPolicy>> GetAllAsync();
        Task<CreditLimitPolicy?> GetByIdAsync(int id);
        Task<CreditLimitPolicy?> GetStudentCreditLimitAsync(int studentId);
        Task<bool> DeleteAsync(int id, int modifiedBy);
    }
}
