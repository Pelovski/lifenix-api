namespace Lifenix.API.DTOs.AuthDTOs
{
    using System.ComponentModel.DataAnnotations;

    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(12, ErrorMessage = "Password must be at least 12 characters.")]
        public string Password { get; set; }
    }
}
