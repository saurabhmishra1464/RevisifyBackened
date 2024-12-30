using revisifyBackened.Models;

namespace revisifyBackened.Interface.IRepository
{
    public interface IQuestionRepository
    {
        Task SaveQuestionsAsync(List<Question> questions);
    }
}
