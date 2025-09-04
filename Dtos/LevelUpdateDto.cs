using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class LevelUpdateDto
    {

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string Description { get; set; }
    }
}
