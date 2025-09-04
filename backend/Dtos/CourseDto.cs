using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class CourseCreateDto
    {
        [Required]
        [StringLength(20)]
        public string Code { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(0.1, 6.0)]
        public decimal CreditHours { get; set; }

        [Required]
        public int LevelId { get; set; }

        public int? ProgramId { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        public bool IsElective { get; set; }
    }

    public class CourseUpdateDto
    {
        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(0.1, 6.0)]
        public decimal CreditHours { get; set; }

        [Required]
        public int LevelId { get; set; }

        public int? ProgramId { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        public bool IsElective { get; set; }

        public bool IsActive { get; set; }
    }

    public class CourseResponseDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal CreditHours { get; set; }
        public int LevelId { get; set; }
        public string LevelName { get; set; }
        public int? ProgramId { get; set; }
        public string ProgramName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public bool IsElective { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }

    public class CourseListResponseDto
    {
        public IEnumerable<CourseResponseDto> Courses { get; set; }
        public int TotalCount { get; set; }
    }

   


    public class BulkUploadResultDto
    {
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
        public List<BulkUploadErrorDto> Errors { get; set; }
    }

    public class BulkUploadErrorDto
    {
        public int Index { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }
}