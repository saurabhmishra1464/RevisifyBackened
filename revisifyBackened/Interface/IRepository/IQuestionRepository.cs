using revisifyBackened.Models;
using revisifyBackened.Models.Dto;

namespace revisifyBackened.Interface.IRepository
{
    public interface IQuestionRepository
    {
        Task SaveQuestionsAsync(List<Question> questions);
        Task<Question> GetQuestionByIdAsync(int questionId);
        Task SaveImageAsync(Question question);
        Task<IEnumerable<Subject>> GetAllSubjectsAsync();
        Task<ApiResponse<List<QuestionDto>>> GetAllQuestionsAsync(int subjectId);
    }
}
