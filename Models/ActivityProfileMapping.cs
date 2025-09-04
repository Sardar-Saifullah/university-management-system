namespace backend.Models
{
    public class ActivityProfileMapping : BaseModel
    {
        public int ProfileId { get; set; }
        public int ActivityId { get; set; }
        public bool CanCreate { get; set; } = false;
        public bool CanRead { get; set; } = false;
        public bool CanUpdate { get; set; } = false;
        public bool CanDelete { get; set; } = false;
        public bool CanExport { get; set; } = false;

        // Navigation properties
        public Profile Profile { get; set; }
        public Activity Activity { get; set; }
    }
}