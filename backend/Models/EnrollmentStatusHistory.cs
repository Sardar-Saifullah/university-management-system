namespace backend.Models
{
    public class EnrollmentStatusHistory : BaseModel
    {
        public int EnrollmentId { get; set; }
        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
        public int? ModifiedBy { get; set; }
        public string Reason { get; set; }
    }
}