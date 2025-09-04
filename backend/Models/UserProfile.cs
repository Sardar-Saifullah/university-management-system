namespace backend.Models
{
    public class UserProfile
    {
       
            public int UserId { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string ProfileName { get; set; } = string.Empty;
            public string? Contact { get; set; }
          
            public string? ProfilePicturePath { get; set; }
    }
          
        
    
}
