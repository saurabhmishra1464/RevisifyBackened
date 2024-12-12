namespace revisifyBackened.ExceptionHandelling.CustomException
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}
