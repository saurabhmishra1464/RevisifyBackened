namespace revisifyBackened
{
    public class JwtSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessTokenExpiry { get; set; }

        public TimeSpan Expire => TimeSpan.FromMinutes(AccessTokenExpiry);
    }
}
