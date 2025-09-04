namespace backend.Models
{
    public class Enrollment : BaseModel
    {
        public int StudentId { get; set; }
        public int CourseOfferingId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string Status { get; set; } = "pending";
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string RejectionReason { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? WithdrawalDate { get; set; }
        public DateTime? DropDate { get; set; }
    }
}