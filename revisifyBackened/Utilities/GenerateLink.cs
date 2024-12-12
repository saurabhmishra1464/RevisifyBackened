using System.Web;

namespace revisifyBackened.Utilities
{
    public  class GenerateLink
    {
        private readonly IConfiguration _configuration;
        public GenerateLink(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public  string GenerateResetPasswordLink(string email, string token)
        {
            var frontendUrl = _configuration["BaseUrls:Frontend"];
            var encodedToken = HttpUtility.UrlEncode(token);
            var encodedEmail = HttpUtility.UrlEncode(email);

            return $"{frontendUrl}/auth/resetPassword?token={encodedToken}&email={encodedEmail}";
        }

        public string GenerateVerifyEmailLink(string email, string token)
        {
            var frontendUrl = _configuration["BaseUrls:Frontend"];
            var encodedToken = HttpUtility.UrlEncode(token);
            var encodedEmail = HttpUtility.UrlEncode(email);

            return $"{frontendUrl}/auth/verifyEmail?token={encodedToken}&email={encodedEmail}";
        }
    }
}
