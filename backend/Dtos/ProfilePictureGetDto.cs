namespace backend.Dtos
{
    public class ProfilePictureGetDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserType { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public string MimeType { get; set; }
        public string StorageType { get; set; }
        public bool IsActive { get; set; }
        public DateTime UploadedAt { get; set; }
        public string UploadedByName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedByName { get; set; }
    }
}