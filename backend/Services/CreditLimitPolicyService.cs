// Services/Implementations/CreditLimitPolicyService.cs
using backend.Dtos;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class CreditLimitPolicyService : ICreditLimitPolicyService
    {
        private readonly ICreditLimitPolicyRepository _policyRepository;
        private readonly ICreditLimitOverrideRepository _overrideRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly ICourseRepository _courseRepository;

        public CreditLimitPolicyService(
            ICreditLimitPolicyRepository policyRepository,
            ICreditLimitOverrideRepository overrideRepository,
            IProfileRepository profileRepository,
            ICourseRepository courseRepository)
        {
            _policyRepository = policyRepository;
            _overrideRepository = overrideRepository;
            _profileRepository = profileRepository;
            _courseRepository = courseRepository;
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
                MinCredits = dto.MinCredits,
                CreatedBy = createdBy
            };

            var id = await _policyRepository.CreateAsync(policy, createdBy);
            var createdPolicy = await _policyRepository.GetByIdAsync(id);

            return MapToResponseDto(createdPolicy!);
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
            existingPolicy.ModifiedBy = modifiedBy;

            var success = await _policyRepository.UpdateAsync(existingPolicy, modifiedBy);
            if (!success)
                throw new Exception("Failed to update credit limit policy");

            var updatedPolicy = await _policyRepository.GetByIdAsync(id);
            return MapToResponseDto(updatedPolicy!);
        }

        public async Task<IEnumerable<CreditLimitPolicyResponseDto>> GetAllAsync()
        {
            var policies = await _policyRepository.GetAllAsync();
            return policies.Select(MapToResponseDto);
        }

        public async Task<CreditLimitPolicyResponseDto?> GetByIdAsync(int id)
        {
            var policy = await _policyRepository.GetByIdAsync(id);
            return policy == null ? null : MapToResponseDto(policy);
        }

        public async Task<bool> DeleteAsync(int id, int modifiedBy)
        {
            return await _policyRepository.DeleteAsync(id, modifiedBy);
        }

        public async Task<EffectiveCreditLimit> GetEffectiveCreditLimitAsync(int studentId)
        {
            // Check if student exists
            var student = await _profileRepository.GetStudentProfile(studentId);
            if (student == null)
                throw new KeyNotFoundException($"Student with ID {studentId} not found");

            // Get the default policy for the student
            var defaultPolicy = await _policyRepository.GetStudentCreditLimitAsync(studentId);
            if (defaultPolicy == null)
                throw new Exception("No credit limit policy found for student");

            // Check for active override
            var activeOverride = await _overrideRepository.GetActiveOverrideForStudentAsync(studentId);

            int effectiveMaxCredits;
            if (activeOverride != null)
            {
                effectiveMaxCredits = activeOverride.NewMaxCredits;
            }
            else
            {
                effectiveMaxCredits = defaultPolicy.MaxCredits;
            }

            return new EffectiveCreditLimit
            {
                EffectiveMaxCredits = effectiveMaxCredits,
                MinRequiredCredits = defaultPolicy.MinCredits,
                HasOverride = activeOverride != null,
                CompletedCredits = student.CompletedCreditHours,
                CurrentSemesterCredits = student.CurrentCreditHours,
                AppliesToName = defaultPolicy.Name,
                PolicyName = defaultPolicy.Name
            };
        }

        public async Task<EnrollmentValidationResult> ValidateEnrollmentAgainstCreditLimit(int studentId, int courseId)
        {
            // Get student and course information
            var student = await _profileRepository.GetStudentProfile(studentId);
            if (student == null)
                throw new KeyNotFoundException($"Student with ID {studentId} not found");

            var course = await _courseRepository.GetCourseById(courseId);
            if (course == null)
                throw new KeyNotFoundException($"Course with ID {courseId} not found");

            // Get effective credit limit
            var creditLimit = await GetEffectiveCreditLimitAsync(studentId);

            // Check if adding this course would exceed the limit
            var totalAfterEnrollment = student.CurrentCreditHours + course.CreditHours;
            var isAllowed = totalAfterEnrollment <= creditLimit.EffectiveMaxCredits;

            return new EnrollmentValidationResult
            {
                IsAllowed = isAllowed,
                Reason = isAllowed
                    ? "Within credit limits"
                    : $"Adding this course would exceed your maximum allowed credits ({creditLimit.EffectiveMaxCredits}). Current credits: {student.CurrentCreditHours}, Course credits: {course.CreditHours}",
                CurrentCredits = student.CurrentCreditHours,
                CourseCredits = course.CreditHours,
                MaxAllowedCredits = creditLimit.EffectiveMaxCredits
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
                MaxCredits = policy.MaxCredits,
                MinCredits = policy.MinCredits,
                IsActive = policy.IsActive,
                CreatedAt = policy.CreatedAt,
                CreatedByName = null // Would need user service to populate this
            };
        }
    }
}