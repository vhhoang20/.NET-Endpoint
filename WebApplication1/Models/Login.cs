using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Login
    {
        [Required(ErrorMessage = "The UserName field is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The Password field is required.")]
        public string Password { get; set; }
    }
}
