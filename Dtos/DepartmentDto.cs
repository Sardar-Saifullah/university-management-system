// Dtos/DepartmentDto.cs
using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class DepartmentResponseDto
    {
        public int Id { get; set; }

       
        public string Name { get; set; }

        
        public string Code { get; set; }
        // Add these properties to match the mapping
        public DateTime CreatedAt { get; set; }
     
        public DateTime ModifiedAt { get; set; }
      
       
    }

    public class DepartmentCreateDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Code is required")]
        [StringLength(20, ErrorMessage = "Code cannot exceed 20 characters")]
        public string Code { get; set; }
    }
    public class DepartmentUpdateDto : DepartmentCreateDto
    {
        [Required]
        public int Id { get; set; }

    }
}

