using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class OrderItem
    {
        [ForeignKey("Order")]
        public int orderID { get; set; }
        [ForeignKey("Cart")]
        public int cartID { get; set; }
        [ForeignKey("Product")]
        public int productID { get; set; }
        public int quantity { get; set; }
        public string status { get; set; }
    }
}
