using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace revisifyBackened.Models
{
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int SubjectId { get; set; }
        [Required]
        public string QuestionText { get; set; }
        public List<Option> Options { get; set; }
        [Required]
        public string CorrectOption { get; set; }
        public Subject Subject { get; set; }
        public string CodeHash { get; set; }
        public string? ImageUrl { get; set; }
        public string Difficulty { get; set; }
    }
}
