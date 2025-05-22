namespace SQRBackend.Models
{
    public class Production
    {
        public long Id { get; set; }
        public string? Email { get; set; }
        public string? OrderId { get; set; }
        public DateTime Date { get; set; }
        public decimal Quantity { get; set; }
        public string? MaterialCode { get; set; }
        public decimal CycleTime { get; set; }
        public User? User { get; set; }
        public Order? Order { get; set; }
    }
}