using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Product
    {
        [Key]
        public int productId { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public int price { get; set; }
        public float rate { get; set; }
    }
}
