namespace backend.Models
{
    public class CreditLimitPolicy : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string AppliesTo { get; set; } = "all";
        public int? AppliesToId { get; set; }
        public int MaxCredits { get; set; }
        public int MinCredits { get; set; } = 0;
    }
}