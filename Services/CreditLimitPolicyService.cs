// Services/CreditLimitPolicyService.cs
using backend.Data;
using backend.Dtos;
using backend.Models;
using backend.Repositories;
using MySql.Data.MySqlClient;
using System.Data;

namespace backend.Services
{
    public class CreditLimitPolicyService : ICreditLimitPolicyService
    {
        private readonly ICreditLimitPolicyRepository _policyRepository;
        private readonly IDatabaseContext _context;

        public CreditLimitPolicyService(ICreditLimitPolicyRepository policyRepository, IDatabaseContext context)
        {
            _policyRepository = policyRepository;
            _context = context;
        }

        public async Task<CreditLimitPolicyResponseDto> CreateAsync(CreditLimitPolicyCreateDto dto, int createdBy)
        {
            var policy = new CreditLimitPolicy
            {
                Name = dto.Name,
                Description = dto.Description,
                AppliesTo = dto.AppliesTo,
                AppliesToId = dto.AppliesToId,
                MaxCredits = dto.MaxCredits,
                MinCredits = dto.MinCredits
            };

            var policyId = await _policyRepository.CreateAsync(policy, createdBy);
            var createdPolicy = await _policyRepository.GetByIdAsync(policyId);

            return MapToResponseDto(createdPolicy);
        }

        public async Task<CreditLimitPolicyResponseDto> UpdateAsync(int id, CreditLimitPolicyUpdateDto dto, int modifiedBy)
        {
            var existingPolicy = await _policyRepository.GetByIdAsync(id);
            if (existingPolicy == null)
                throw new KeyNotFoundException($"Credit limit policy with ID {id} not found");

            existingPolicy.Name = dto.Name;
            existingPolicy.Description = dto.Description;
            existingPolicy.AppliesTo = dto.AppliesTo;
            existingPolicy.AppliesToId = dto.AppliesToId;
            existingPolicy.MaxCredits = dto.MaxCredits;
            existingPolicy.MinCredits = dto.MinCredits;
            existingPolicy.IsActive = dto.IsActive;

            await _policyRepository.UpdateAsync(existingPolicy, modifiedBy);
            var updatedPolicy = await _policyRepository.GetByIdAsync(id);

            return MapToResponseDto(updatedPolicy);
        }

        public async Task<IEnumerable<CreditLimitPolicyResponseDto>> GetAllAsync()
        {
            var policies = await _policyRepository.GetAllAsync();
            return policies.Select(MapToResponseDto);
        }

        public async Task<CreditLimitPolicyResponseDto?> GetByIdAsync(int id)
        {
            var policy = await _policyRepository.GetByIdAsync(id);
            return policy != null ? MapToResponseDto(policy) : null;
        }

        public async Task<bool> DeleteAsync(int id, int modifiedBy)
        {
            return await _policyRepository.DeleteAsync(id, modifiedBy);
        }

        public async Task<EffectiveCreditLimitResponseDto> GetEffectiveCreditLimitAsync(int studentId)
        {
            var parameters = new[] { new MySqlParameter("p_student_id", studentId) };
            var dataTable = await _context.ExecuteQueryAsync("sp_get_effective_credit_limit", parameters);

            if (dataTable.Rows.Count == 0)
                throw new KeyNotFoundException($"Student with ID {studentId} not found or no credit limit policy applies");

            var row = dataTable.Rows[0];

            return new EffectiveCreditLimitResponseDto
            {
                EffectiveMaxCredits = Convert.ToInt32(row["effective_max_credits"]),
                MinRequiredCredits = Convert.ToInt32(row["min_required_credits"]),
                HasOverride = Convert.ToBoolean(row["has_override"]),
                CompletedCredits = Convert.ToDecimal(row["completed_credits"]),
                CurrentSemesterCredits = Convert.ToDecimal(row["current_semester_credits"]),
                AppliesToName = row["applies_to_name"].ToString(),
                PolicyName = row["policy_name"].ToString()
            };
        }

       

        private CreditLimitPolicyResponseDto MapToResponseDto(CreditLimitPolicy policy)
        {
            return new CreditLimitPolicyResponseDto
            {
                Id = policy.Id,
                Name = policy.Name,
                Description = policy.Description,
                AppliesTo = policy.AppliesTo,
                AppliesToId = policy.AppliesToId,
                AppliesToName = policy.AppliesToName,
                MaxCredits = policy.MaxCredits,
                MinCredits = policy.MinCredits,
                IsActive = policy.IsActive,
                CreatedAt = policy.CreatedAt,
                CreatedByName = policy.CreatedByName, // This should be set from the stored procedure
                ModifiedAt = policy.ModifiedAt,
                ModifiedByName = policy.ModifiedByName // This should be set from the stored procedure
            };
        }
    }
}