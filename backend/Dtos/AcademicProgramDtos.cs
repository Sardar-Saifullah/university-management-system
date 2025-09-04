// Dtos/ProgramDtos.cs
using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class ProgramCreateDto
    {
        [Required]
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        public string Code { get; set; }

        [Range(1, 20)]
        public int DurationSemesters { get; set; } = 8;

        [Range(1, 500)]
        public int CreditHoursRequired { get; set; } = 130;

        public string Description { get; set; }
    }

    public class ProgramUpdateDto : ProgramCreateDto
    {
        [Required]
        public int Id { get; set; }
    }

    public class ProgramResponseDto
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int DurationSemesters { get; set; }
        public int CreditHoursRequired { get; set; }
        public string Description { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}