namespace revisifyBackened
{
    public class JWTService
    {
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? Secret { get; set; }
        public int AccessTokenExpiry { get; set; }
        public int RefreshTokenExpiry { get; set; }
    }
}
