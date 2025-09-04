using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class ProfilePictureCreateDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public IFormFile ImageFile { get; set; }
    }
}