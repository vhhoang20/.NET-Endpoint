using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class User : IdentityUser
    {
        public string? name { get; set; }
        [Required(ErrorMessage = "The Email field is required.")]
        public override string Email { get; set; }
        public DateTime? birth { get; set; }
        public string? gender { get; set; }
        public string? company { get; set; }
        public string? home { get; set; }

        /*public string username { get; set; }
        public string password { get; set; }*/
    }
}
