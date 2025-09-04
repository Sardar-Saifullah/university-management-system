namespace backend.Models
{
    public class AdminProfile : BaseModel
    {
        public int UserId { get; set; }
        public string Level { get; set; } = "super_admin"; // super_admin, admin, moderator
        public string AccessLevel { get; set; }
        public DateTime HireDate { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? ProfilePictureId { get; set; }
    }
}   