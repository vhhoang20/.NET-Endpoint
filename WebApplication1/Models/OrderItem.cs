using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class OrderItem
    {
        [Key]
        public int orderID { get; set; }
        [Key]
        public int cartID { get; set; }
        public int productID { get; set; }
        public int quantity { get; set; }
        public string status { get; set; }
    }
}
