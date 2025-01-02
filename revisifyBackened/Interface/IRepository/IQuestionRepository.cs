using revisifyBackened.Models;

namespace revisifyBackened.Interface.IRepository
{
    public interface IQuestionRepository
    {
        Task SaveQuestionsAsync(List<Question> questions);
        Task<Question> GetQuestionByIdAsync(int questionId);
        Task SaveImageAsync(Question question);
        Task<IEnumerable<Subject>> GetAllSubjectsAsync();
    }
}
