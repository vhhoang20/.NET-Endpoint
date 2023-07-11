namespace WebApplication1.Models
{
    public class Order
    {
        public int ID { get; set; }
        public int customerID { get; set; }
        public DateOnly date { get; set; }
        public string address { get; set; }
        public int price { get; set; }
        public string status { get; set; }
    }
}
