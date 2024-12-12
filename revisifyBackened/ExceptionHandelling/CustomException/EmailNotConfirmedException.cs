namespace revisifyBackened.ExceptionHandelling.CustomException
{
    public class EmailNotConfirmedException : Exception
    {
        public EmailNotConfirmedException() : base("Email not confirmed. Please confirm your email to proceed.") { }
    }

}
