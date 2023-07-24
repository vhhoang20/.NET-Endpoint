using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Cart
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("User")]
        public string customerID { get; set; }
        public User User { get; set; }
    }
}
