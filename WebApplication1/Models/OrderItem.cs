using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class OrderItem
    {
        [Key]
        public int orderID { get; set; }
        [Key]
        public int cartID { get; set; }
        [ForeignKey("Product")]
        public int productID { get; set; }
        public int quantity { get; set; }
        public string status { get; set; }
    }
}
