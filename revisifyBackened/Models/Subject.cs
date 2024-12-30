using System.ComponentModel.DataAnnotations;

namespace revisifyBackened.Models
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
