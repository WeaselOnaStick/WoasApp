using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WoasApp.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email Required!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Passowrd Required!")]
        [PasswordPropertyText]
        public string Password { get; set; }
        
    }
}
