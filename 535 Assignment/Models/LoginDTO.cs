using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace _535_Assignment.Models
{
    public class LoginDTO
    {
        [Display(Name = "Username:")]
        public string? user { get; set; }
        [Display(Name = "Password:")]
        public string? password { get; set; }
        public string RedirectURL { get; set; }
    }
    public class RegisterDTO : LoginDTO
    {

    }
}
