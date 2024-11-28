using Microsoft.AspNetCore.Identity;

namespace revisifyBackened.Data
{
    public class ApplicationUser: IdentityUser
    {
        public string? FullName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; }
    }
}
