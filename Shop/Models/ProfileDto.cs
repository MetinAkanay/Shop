using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class ProfileDto
    {
        [Required(ErrorMessage = "The FirstName field is required"), MaxLength(100)]
        public string FirstName { get; set; } = "";


        [Required(ErrorMessage = "The LastName field is required"), MaxLength(100)]
        public string LastName { get; set; } = "";


        [EmailAddress(ErrorMessage = "The Email field is required"), MaxLength(100)]
        public string Email { get; set; } = "";


        [Phone(ErrorMessage = "The format of the Phone Number is not valid"), MaxLength(20)]
        public string PhoneNumber { get; set; } = "";


        [Required(ErrorMessage = "The Address field is required"), MaxLength(200)]
        public string Address { get; set; } = "";
    }
}
