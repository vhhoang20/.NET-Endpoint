using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Register
    {
        [Required(ErrorMessage = "The UserName field is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The Password field is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Mail { get; set; }
    }
}
