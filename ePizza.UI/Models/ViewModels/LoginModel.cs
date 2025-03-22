using System.ComponentModel.DataAnnotations;

namespace ePizza.UI.Models.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter the user name")]
        [EmailAddress(ErrorMessage = "Email Address is not in correct format")]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
