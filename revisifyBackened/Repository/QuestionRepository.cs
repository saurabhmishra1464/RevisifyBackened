using Microsoft.EntityFrameworkCore;
using revisifyBackened.Data;
using revisifyBackened.Interface.IRepository;
using revisifyBackened.Models;
using revisifyBackened.Models.Dto;

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

        public async Task<IEnumerable<Subject>> GetAllSubjectsAsync()
        {
            return await _dbContext.Subjects.ToListAsync();
        }

        public async Task<ApiResponse<List<QuestionDto>>> GetAllQuestionsAsync(int subjectId)
        {
            var questions = await _dbContext.Questions
                .Where(q => q.SubjectId == subjectId)
                .Select(q => new QuestionDto
                {
                    Id = q.Id,
                    QuestionText = q.QuestionText,
                    CorrectOption = q.CorrectOption,
                    CodeHash = q.CodeHash,
                    ImageUrl = q.ImageUrl,
                    Difficulty = q.Difficulty,
                    SubjectId = q.SubjectId,
                    Options = _dbContext.Options
                        .Where(o => o.QuestionId == q.Id)
                        .Select(o => new OptionDto
                        {
                            Id = o.Id,
                            OptionText = o.OptionText,
                            Value = o.Value
                        })
                        .ToList()
                })
                .ToListAsync();

            return new ApiResponse<List<QuestionDto>>(questions);
        }

    }
}
