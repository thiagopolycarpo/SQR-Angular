namespace SQRBackend.Models
{
    public class Order
    {
        public string? OrderId { get; set; }
        public decimal Quantity { get; set; }
        public string? ProductCode { get; set; }
        public Product? Product { get; set; }
    }
}