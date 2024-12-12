namespace revisifyBackened.Models.Dto
{
    public class UserProfile
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string? UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public List<string>? Roles { get; set; }
        public bool? IEmailConfirmed { get; set; }
    }
}
