// Services/ICreditLimitOverrideService.cs
using backend.Dtos;

namespace backend.Services
{
    public interface ICreditLimitOverrideService
    {
        Task<CreditLimitOverrideResponseDto> CreateAsync(CreditLimitOverrideCreateDto dto, int approvedByUserId);
        Task<CreditLimitOverrideResponseDto> UpdateAsync(int id, CreditLimitOverrideUpdateDto dto, int modifiedBy);
        Task<IEnumerable<CreditLimitOverrideResponseDto>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<CreditLimitOverrideResponseDto>> GetAllAsync(bool? activeOnly = true, int? departmentId = null, int? programId = null);
        Task<CreditLimitOverrideResponseDto?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id, int modifiedBy);
    }
}
