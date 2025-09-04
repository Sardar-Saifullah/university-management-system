namespace backend.Models
{
    public class User : BaseModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int ProfileId { get; set; }
        
        public string? Contact { get; set; }
    }
}