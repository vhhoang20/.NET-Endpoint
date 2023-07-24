using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class ShoppingItem
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Cart")]
        public int? cartId { get; set; }

        [ForeignKey("Order")]
        public int? orderId { get; set; }

        [ForeignKey("Product")]
        public int productId { get; set; }
        public int product { get; set; }
        public int quantity { get; set; }
        public string status { get; set; }
    }
}
