namespace backend.Models
{
    public class UserSession : BaseModel
    {
        public string SessionId { get; set; }
        public int UserId { get; set; }
        public string Jti { get; set; }
        public string DeviceInfo { get; set; }
        public DateTime LoginAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; } = false;
        public DateTime? RevokedAt { get; set; }
        public DateTime LastActivity { get; set; }
    }
}