using System.ComponentModel.DataAnnotations;

namespace revisifyBackened.Models.Dto
{
    public class RegistrationRequestDto
    {

        [Required(ErrorMessage = "Full name is required")]
        [StringLength(50, ErrorMessage = "Full name cannot exceed 50 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number")]
        public string Password { get; set; }
    }
}
