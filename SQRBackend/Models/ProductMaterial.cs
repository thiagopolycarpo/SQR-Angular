namespace SQRBackend.Models
{
    public class ProductMaterial
    {
        public string? ProductCode { get; set; }
        public string? MaterialCode { get; set; }
        public Product? Product { get; set; }
        public Material? Material { get; set; }
    }
}