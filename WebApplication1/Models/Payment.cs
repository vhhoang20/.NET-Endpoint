using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Payment
    {
        [Key]
        public int paymentId { get; set; }
        [ForeignKey("User")]
        public string customerID { get; set; }
        public User User { get; set; }
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public int balance { get; set; }
    }
}
