namespace backend.Models
{
    public class EnrollmentValidationResult
    {
        public bool IsAllowed { get; set; }
        public string Reason { get; set; } = string.Empty;
        public decimal CurrentCredits { get; set; }
        public decimal CourseCredits { get; set; }
        public int MaxAllowedCredits { get; set; }
    }
}
