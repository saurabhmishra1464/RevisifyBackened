namespace revisifyBackened.Models.Dto
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public string CorrectOption { get; set; }
        public string CodeHash { get; set; }
        public string ImageUrl { get; set; }
        public string Difficulty { get; set; }
        public int SubjectId { get; set; }
        public List<OptionDto> Options { get; set; }
    }


}
