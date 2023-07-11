namespace WebApplication1.Models
{
    public class OrderItem
    {
        public int orderID { get; set; }
        public int cartID { get; set; }
        public int productID { get; set; }
        public int quantity { get; set; }
        public string status { get; set; }
    }
}
