using System.ComponentModel.DataAnnotations;

namespace DotNetCoreWithIdentityServer.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public required string Email { get; set; }
    }
}
