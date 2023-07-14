using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Cart
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("User")]
        public int customerID { get; set; }
    }
}
