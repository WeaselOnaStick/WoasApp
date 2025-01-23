using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WoasApp.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username Required!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Passowrd Required!")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Remember Me")]
        public bool Remember { get; set; }

    }
}
