using Microsoft.EntityFrameworkCore;
using revisifyBackened.Data;
using revisifyBackened.Interface.IRepository;
using revisifyBackened.Models;

namespace revisifyBackened.Repository
{
    public class QuestionRepository: IQuestionRepository
    {
        private readonly RevisifyContext _dbContext;

        public QuestionRepository(RevisifyContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveQuestionsAsync(List<Question> questions)
        {
            foreach (var question in questions)
            {
                foreach (var option in question.Options)
                {
                    option.QuestionId = question.Id; // Use QuestionId instead
                }
                _dbContext.Questions.Add(question);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task SaveImageAsync(Question question)
        {

            _dbContext.Questions.Update(question);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Question> GetQuestionByIdAsync(int questionId)
        {
            return await _dbContext.Questions
                .FirstOrDefaultAsync(q => q.Id == questionId);
        }

    }
}
