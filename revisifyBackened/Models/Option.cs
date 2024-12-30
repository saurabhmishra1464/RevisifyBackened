using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace revisifyBackened.Models
{
    public class Option
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int QuestionId { get; set; }
        [Required]
        public string OptionText { get; set; }
        public string Value { get; set; }
        public Question Question { get; set; }
    }
}
