using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Order
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("User")]
        public int customerID { get; set; }
        public DateTime date { get; set; }
        public string address { get; set; }
        public int price { get; set; }
        public string status { get; set; }
    }
}
