// Dtos/CreditLimitPolicy/CreditLimitPolicyCreateDto.cs
using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class CreditLimitPolicyCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        [RegularExpression("^(all|program|level|department)$")]
        public string AppliesTo { get; set; } = "all";

        public int? AppliesToId { get; set; }

        [Required]
        [Range(1, 30)]
        public int MaxCredits { get; set; }

        [Range(0, 30)]
        public int MinCredits { get; set; } = 0;
    }

    public class CreditLimitPolicyUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        [RegularExpression("^(all|program|level|department)$")]
        public string AppliesTo { get; set; } = "all";

        public int? AppliesToId { get; set; }

        [Required]
        [Range(1, 30)]
        public int MaxCredits { get; set; }

        [Range(0, 30)]
        public int MinCredits { get; set; } = 0;

        [Required]
        public bool IsActive { get; set; } = true;
    }

    public class CreditLimitPolicyResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string AppliesTo { get; set; } = "all";
        public int? AppliesToId { get; set; }
        public string? AppliesToName { get; set; }
        public int MaxCredits { get; set; }
        public int MinCredits { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedByName { get; set; }
    }

    public class CreditLimitOverrideCreateDto
    {
        [Required]
        public int PolicyId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        [Range(1, 30)]
        public int NewMaxCredits { get; set; }

        [Required]
        [StringLength(500)]
        public string Reason { get; set; } = string.Empty;

        public DateTime? ExpiresAt { get; set; }
    }


    public class CreditLimitOverrideUpdateDto
    {
        [Required]
        [Range(1, 30)]
        public int NewMaxCredits { get; set; }

        [Required]
        [StringLength(500)]
        public string Reason { get; set; } = string.Empty;

        public DateTime? ExpiresAt { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
    }



    public class CreditLimitOverrideResponseDto
    {
        public int Id { get; set; }
        public int PolicyId { get; set; }
        public string PolicyName { get; set; } = string.Empty;
        public int StudentId { get; set; }
        public string StudentRegNo { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public int NewMaxCredits { get; set; }
        public int DefaultMaxCredits { get; set; }
        public string Reason { get; set; } = string.Empty;
        public int ApprovedBy { get; set; }
        public string ApprovedByName { get; set; } = string.Empty;
        public DateTime? ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class EffectiveCreditLimitResponseDto
    {
        public int EffectiveMaxCredits { get; set; }
        public int MinRequiredCredits { get; set; }
        public bool HasOverride { get; set; }
        public decimal CompletedCredits { get; set; }
        public decimal CurrentSemesterCredits { get; set; }
        public string? AppliesToName { get; set; }
        public string? PolicyName { get; set; }
    }



    public class EnrollmentValidationResponseDto
    {
        public bool IsAllowed { get; set; }
        public string Reason { get; set; } = string.Empty;
        public decimal CurrentCredits { get; set; }
        public decimal CourseCredits { get; set; }
        public int MaxAllowedCredits { get; set; }
    }
}