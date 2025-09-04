namespace backend.Models
{
    public class EffectiveCreditLimit
    {
        public int EffectiveMaxCredits { get; set; }
        public int MinRequiredCredits { get; set; }
        public bool HasOverride { get; set; }
        public decimal CompletedCredits { get; set; }
        public decimal CurrentSemesterCredits { get; set; }
        public string? AppliesToName { get; set; }
        public string? PolicyName { get; set; }
    }
}
