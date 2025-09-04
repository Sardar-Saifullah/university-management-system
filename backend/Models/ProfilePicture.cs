namespace backend.Models
{
    public class ProfilePicture : BaseModel
    {
        public int UserId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string MimeType { get; set; } = string.Empty;
        public string StorageType { get; set; } = "local";
        public int UploadedBy { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
     
    }
    public class ProfilePictureResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string MimeType { get; set; } = string.Empty;
        public string StorageType { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime UploadedAt { get; set; }
        public string UploadedByName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public DateTime ModifiedAt { get; set; }
        public string ModifiedByName { get; set; } = string.Empty;
    }
}