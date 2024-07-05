using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class RegisterDto
    {
        [Required(ErrorMessage ="The FirstName field is required"), MaxLength(100)]
        public string FirstName { get; set; } = "";


        [Required(ErrorMessage = "The LastName field is required"), MaxLength(100)]
        public string LastName { get; set; } = "";


        [EmailAddress(ErrorMessage = "The Email field is required"), MaxLength(100)]
        public string Email { get; set; } = "";


        [Phone(ErrorMessage = "The format of the Phone Number is not valid"), MaxLength(20)]
        public string PhoneNumber { get; set; } = "";


        [Required(ErrorMessage = "The Address field is required"), MaxLength(200)]
        public string Address { get; set; } = "";


        [Required, MaxLength(100)]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "The Confirm Password field is required")]
        [Compare("Password", ErrorMessage = "The password and confirmation Password do not match.")]
        public string ConfirmPassword { get; set; } = "";

    }
}
