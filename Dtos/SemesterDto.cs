// Dtos/Semester/CreateSemesterDto.cs
using System.ComponentModel.DataAnnotations;
namespace backend.Dtos
{
    
        public class CreateSemesterDto
        {
            [Required(ErrorMessage = "Name is required")]
            [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Start date is required")]
            public DateTime StartDate { get; set; }

            [Required(ErrorMessage = "End date is required")]
            public DateTime EndDate { get; set; }

            [Required(ErrorMessage = "Academic year is required")]
            [StringLength(20, ErrorMessage = "Academic year cannot be longer than 20 characters")]
            public string AcademicYear { get; set; }

            public bool IsCurrent { get; set; } = false;
            public DateTime? RegistrationStart { get; set; }
            public DateTime? RegistrationEnd { get; set; }
        }
    


        public class UpdateSemesterDto
        {
            [Required(ErrorMessage = "Name is required")]
            [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Start date is required")]
            public DateTime StartDate { get; set; }

            [Required(ErrorMessage = "End date is required")]
            public DateTime EndDate { get; set; }

            [Required(ErrorMessage = "Academic year is required")]
            [StringLength(20, ErrorMessage = "Academic year cannot be longer than 20 characters")]
            public string AcademicYear { get; set; }

            public bool IsCurrent { get; set; } = false;
            public DateTime? RegistrationStart { get; set; }
            public DateTime? RegistrationEnd { get; set; }
        }

        public class GetSemesterDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public bool IsCurrent { get; set; }
            public DateTime? RegistrationStart { get; set; }
            public DateTime? RegistrationEnd { get; set; }
            public string AcademicYear { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime ModifiedAt { get; set; }
        }
    }
