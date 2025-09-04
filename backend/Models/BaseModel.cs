namespace backend.Models
{
    public class BaseModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public int? ModifiedBy { get; set; } 
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
    }
}