namespace WebApplication1.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int customerID { get; set; }
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public int balance { get; set; }
    }
}
