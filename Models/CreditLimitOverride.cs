namespace backend.Models
{
    public class CreditLimitOverride : BaseModel
    {
        public int PolicyId { get; set; }
        public int StudentId { get; set; }
        public int NewMaxCredits { get; set; }
        public string Reason { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime? ExpiresAt { get; set; }

        // Navigation properties (will be populated in service layer)
        public string? PolicyName { get; set; }
        public string? StudentName { get; set; }
        public string? StudentRegNo { get; set; }
        public string? ApprovedByName { get; set; }
        public string? Status { get; set; }
    }
}