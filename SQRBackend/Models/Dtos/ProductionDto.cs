namespace SQRBackend.Models.Dtos
{
    public class ProductionDto
    {
        public long Id { get; set; }
        public string? Order { get; set; }
        public string? Date { get; set; }
        public decimal Quantity { get; set; }
        public string? MaterialCode { get; set; }
        public decimal CycleTime { get; set; }
    }
}