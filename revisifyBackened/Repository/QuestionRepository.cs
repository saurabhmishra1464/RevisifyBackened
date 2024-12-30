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
                // Ensure Subject is correctly linked
                //var subject = await _dbContext.Subjects.FindAsync(question.SubjectId);
                //if (subject == null)
                //    throw new Exception($"Subject with ID {question.SubjectId} not found.");

                // Link Options to Question
                foreach (var option in question.Options)
                {
                    option.Question = question; // Establish relationship
                }

                _dbContext.Questions.Add(question);
            }

            await _dbContext.SaveChangesAsync();

        }

    }
}
