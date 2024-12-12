namespace revisifyBackened.ExceptionHandelling.CustomException
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base("User not found") { }
    }
}
