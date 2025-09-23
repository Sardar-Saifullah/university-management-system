// Services/ICreditLimitPolicyService.cs
using backend.Dtos;

namespace backend.Services
{
    public interface ICreditLimitPolicyService
    {
        Task<CreditLimitPolicyResponseDto> CreateAsync(CreditLimitPolicyCreateDto dto, int createdBy);
        Task<CreditLimitPolicyResponseDto> UpdateAsync(int id, CreditLimitPolicyUpdateDto dto, int modifiedBy);
        Task<IEnumerable<CreditLimitPolicyResponseDto>> GetAllAsync();
        Task<CreditLimitPolicyResponseDto?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id, int modifiedBy);
        Task<EffectiveCreditLimitResponseDto> GetEffectiveCreditLimitAsync(int studentId);
       
    }
}