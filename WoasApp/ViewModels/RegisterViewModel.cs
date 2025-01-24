using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WoasApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Username Required!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email Required!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Passowrd Required!")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        [StringLength(16, MinimumLength = 1, ErrorMessage = "Password must be between 1 and 16 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please Repeat Password!")]
        [Compare("Password", ErrorMessage = "Passwords Must Match!")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
