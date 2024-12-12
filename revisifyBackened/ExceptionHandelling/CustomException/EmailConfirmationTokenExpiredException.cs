namespace revisifyBackened.ExceptionHandelling.CustomException
{
    public class EmailConfirmationTokenExpiredException : Exception
    {
        public EmailConfirmationTokenExpiredException() : base("Email confirmation token has expired. Please request a new confirmation email.") { }
    }
}
