// Dtos/CreditLimitPolicy/CreditLimitPolicyCreateDto.cs
using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class CreditLimitPolicyCreateDto
    {
        [Required(ErrorMessage = "Policy name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "AppliesTo is required")]
        [RegularExpression("^(all|program|level|department)$", ErrorMessage = "AppliesTo must be one of: all, program, level, department")]
        public string AppliesTo { get; set; } = "all";

        public int? AppliesToId { get; set; }

        [Required(ErrorMessage = "Max credits is required")]
        [Range(1, 30, ErrorMessage = "Max credits must be between 1 and 30")]
        public int MaxCredits { get; set; }

        [Range(0, 30, ErrorMessage = "Min credits must be between 0 and 30")]
        public int MinCredits { get; set; } = 0;
    }


// Dtos/CreditLimitPolicy/CreditLimitPolicyUpdateDto.cs

    public class CreditLimitPolicyUpdateDto
    {
        [Required(ErrorMessage = "Policy name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "AppliesTo is required")]
        [RegularExpression("^(all|program|level|department)$", ErrorMessage = "AppliesTo must be one of: all, program, level, department")]
        public string AppliesTo { get; set; }

        public int? AppliesToId { get; set; }

        [Required(ErrorMessage = "Max credits is required")]
        [Range(1, 30, ErrorMessage = "Max credits must be between 1 and 30")]
        public int MaxCredits { get; set; }

        [Range(0, 30, ErrorMessage = "Min credits must be between 0 and 30")]
        public int MinCredits { get; set; }

        [Required(ErrorMessage = "IsActive is required")]
        public bool IsActive { get; set; }
    }


// Dtos/CreditLimitPolicy/CreditLimitPolicyResponseDto.cs

    public class CreditLimitPolicyResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AppliesTo { get; set; }
        public int? AppliesToId { get; set; }
        public string AppliesToName { get; set; }
        public int MaxCredits { get; set; }
        public int MinCredits { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByName { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string ModifiedByName { get; set; }
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
        public string AppliesToName { get; set; }
        public string PolicyName { get; set; }
    }

  


}