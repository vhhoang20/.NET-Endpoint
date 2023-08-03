using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class ShoppingItem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }


        [ForeignKey("Cart")]
        public int? CartId { get; set; }


        [ForeignKey("Order")]
        public int? OrderId { get; set; }

        public int quantity { get; set; }
        public string status { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
