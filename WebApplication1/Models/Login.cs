using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Login
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
