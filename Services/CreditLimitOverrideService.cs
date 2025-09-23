// Services/Implementations/CreditLimitOverrideService.cs
using backend.Dtos;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class CreditLimitOverrideService : ICreditLimitOverrideService
    {
        private readonly ICreditLimitOverrideRepository _overrideRepository;
        private readonly ICreditLimitPolicyRepository _policyRepository;
        private readonly IProfileRepository _studentRepository;
        private readonly IProfileRepository _adminRepository;

        public CreditLimitOverrideService(
            ICreditLimitOverrideRepository overrideRepository,
            ICreditLimitPolicyRepository policyRepository,
            IProfileRepository studentRepository,
            IProfileRepository adminRepository)
        {
            _overrideRepository = overrideRepository;
            _policyRepository = policyRepository;
            _studentRepository = studentRepository;
            _adminRepository = adminRepository;
        }

        public async Task<CreditLimitOverrideResponseDto> CreateAsync(CreditLimitOverrideCreateDto dto, int approvedByUserId)
        {
            // Validate policy exists
            var policy = await _policyRepository.GetByIdAsync(dto.PolicyId);
            if (policy == null)
                throw new KeyNotFoundException($"Credit limit policy with ID {dto.PolicyId} not found");

            // Validate student exists
            var student = await _studentRepository.GetStudentProfile(dto.StudentId);
            if (student == null)
                throw new KeyNotFoundException($"Student with ID {dto.StudentId} not found");

            // Check if user has admin privileges (simpler check without calling admin profile)
            var isAdmin = await _adminRepository.IsUserAdmin(approvedByUserId);
            if (!isAdmin)
                throw new UnauthorizedAccessException("Only admin users can create credit limit overrides");

            var overrideEntity = new CreditLimitOverride
            {
                PolicyId = dto.PolicyId,
                StudentId = dto.StudentId,
                NewMaxCredits = dto.NewMaxCredits,
                Reason = dto.Reason,
                ExpiresAt = dto.ExpiresAt,
                CreatedBy = approvedByUserId,
                ModifiedBy = approvedByUserId
            };

            var id = await _overrideRepository.CreateAsync(overrideEntity, approvedByUserId);
            var createdOverride = await _overrideRepository.GetByIdAsync(id);

            return MapToResponseDto(createdOverride!);
        }

        public async Task<CreditLimitOverrideResponseDto> UpdateAsync(int id, CreditLimitOverrideUpdateDto dto, int modifiedBy)
        {
            var existingOverride = await _overrideRepository.GetByIdAsync(id);
            if (existingOverride == null)
                throw new KeyNotFoundException($"Credit limit override with ID {id} not found");

            // Validate approver is admin
            var admin = await _adminRepository.GetAdminProfile(modifiedBy);
            if (admin == null)
                throw new UnauthorizedAccessException("Only admin users can update credit limit overrides");

            existingOverride.NewMaxCredits = dto.NewMaxCredits;
            existingOverride.Reason = dto.Reason;
            existingOverride.ExpiresAt = dto.ExpiresAt;
            existingOverride.IsActive = dto.IsActive;
            existingOverride.ModifiedBy = modifiedBy;

            var success = await _overrideRepository.UpdateAsync(existingOverride, modifiedBy);
            if (!success)
                throw new Exception("Failed to update credit limit override");

            var updatedOverride = await _overrideRepository.GetByIdAsync(id);
            return MapToResponseDto(updatedOverride!);
        }

        public async Task<IEnumerable<CreditLimitOverrideResponseDto>> GetByStudentIdAsync(int studentId)
        {
            var overrides = await _overrideRepository.GetByStudentIdAsync(studentId);
            return overrides.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<CreditLimitOverrideResponseDto>> GetAllAsync(bool? activeOnly = true, int? departmentId = null, int? programId = null)
        {
            var overrides = await _overrideRepository.GetAllAsync(activeOnly, departmentId, programId);
            return overrides.Select(MapToResponseDto);
        }

        public async Task<CreditLimitOverrideResponseDto?> GetByIdAsync(int id)
        {
            var overrideEntity = await _overrideRepository.GetByIdAsync(id);
            return overrideEntity == null ? null : MapToResponseDto(overrideEntity);
        }

        public async Task<bool> DeleteAsync(int id, int modifiedBy)
        {
            // Check if override exists
            var existingOverride = await _overrideRepository.GetByIdAsync(id);
            if (existingOverride == null)
                throw new KeyNotFoundException($"Credit limit override with ID {id} not found");

            // Validate user is admin
            var isAdmin = await _adminRepository.IsUserAdmin(modifiedBy);
            if (!isAdmin)
                throw new UnauthorizedAccessException("Only admin users can delete credit limit overrides");

            // Perform deletion
            return await _overrideRepository.DeleteAsync(id, modifiedBy);
        }

        private CreditLimitOverrideResponseDto MapToResponseDto(CreditLimitOverride overrideEntity)
        {
            return new CreditLimitOverrideResponseDto
            {
                Id = overrideEntity.Id,
                PolicyId = overrideEntity.PolicyId,
                PolicyName = overrideEntity.PolicyName ?? "Unknown Policy",
                StudentId = overrideEntity.StudentId,
                StudentRegNo = overrideEntity.StudentRegNo ?? "Unknown",
                StudentName = overrideEntity.StudentName ?? "Unknown Student",
                NewMaxCredits = overrideEntity.NewMaxCredits,
                DefaultMaxCredits = 0, // You might want to fetch this from the actual policy
                Reason = overrideEntity.Reason,
                ApprovedBy = overrideEntity.ApprovedBy,
                ApprovedByName = overrideEntity.ApprovedByName ?? "Unknown Admin",
                ExpiresAt = overrideEntity.ExpiresAt,
                CreatedAt = overrideEntity.CreatedAt,
                IsActive = overrideEntity.IsActive,
                Status = overrideEntity.Status ?? "Unknown"
            };
        }
    }
}