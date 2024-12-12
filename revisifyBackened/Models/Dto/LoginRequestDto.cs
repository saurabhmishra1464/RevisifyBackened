using System.ComponentModel.DataAnnotations;

namespace revisifyBackened.Models.Dto
{
    public class LoginRequestDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string UserName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The password must be between 6 and 100 characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
