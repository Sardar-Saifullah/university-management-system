using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{

    public class TeacherAssignmentCreateDto
    {
        [Required]
        public int TeacherId { get; set; }

        [Required]
        public int CourseOfferingId { get; set; }

        public bool IsPrimary { get; set; } = true;
    }
    public class TeacherAssignmentUpdateDto
    {
        [Required]
        public bool IsPrimary { get; set; }
    }
    public class TeacherAssignmentResponseDto
    {
        public int Id { get; set; }
        public int TeacherId { get; set; } = 0;
        public string TeacherName { get; set; } = string.Empty;
        public int CourseOfferingId { get; set; }
        public string CourseCode { get; set; }
        public string CourseTitle { get; set; }
        public string SemesterName { get; set; }
        public bool IsPrimary { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool IsActive { get; set; }
    }
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string message = "Operation successful")
        {
            return new ApiResponse<T> { Success = true, Message = message, Data = data };
        }

        public static ApiResponse<T> ErrorResponse(string message, T data = default)
        {
            return new ApiResponse<T> { Success = false, Message = message, Data = data };
        }
    }

}
