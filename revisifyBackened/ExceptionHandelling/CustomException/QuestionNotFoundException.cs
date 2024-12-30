namespace revisifyBackened.ExceptionHandelling.CustomException
{
    public class QuestionNotFoundException : Exception
    {
        public QuestionNotFoundException() : base("Question not found") { }
    }
}
