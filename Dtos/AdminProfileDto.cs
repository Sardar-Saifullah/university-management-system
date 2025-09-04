using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class AdminProfileDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string ProfilePicUrl { get; set; }
        public string ProfilePicType { get; set; }
        public string Level { get; set; }
        public DateTime HireDate { get; set; }
        public string DepartmentName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
    public class AdminOwnProfileUpdateDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
    }

}
