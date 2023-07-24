using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class User : IdentityUser
    {
        [Key]
        public int ID { get; set; }
        public string? name { get; set; }
        public string mail { get; set; }
        public DateTime? birth { get; set; }
        public bool? sex { get; set; }
        public string? company { get; set; }
        public string? home { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
