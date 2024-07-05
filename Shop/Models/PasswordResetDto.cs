using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class PasswordResetDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required, MaxLength(100)]
        public string Password { get; set; } = "";

        [Required(ErrorMessage ="The Confirm Password field is required")]
        [Compare("Password", ErrorMessage = "The password and confirmation Password do not match.")]
        public string ConfirmPassword { get; set; } = "";
    }
}
