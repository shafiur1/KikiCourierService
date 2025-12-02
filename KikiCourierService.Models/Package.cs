
namespace KikiCourierService.Models
{
    public class Package
    {
        public string Id { get; init; } = null!;
        public double Weight { get; init; }
        public double Distance { get; init; }
        public string? OfferCode { get; init; }
        public double Discount { get; set; }
        public double TotalCost { get; set; }
        public double DeliveryTime { get; set; }
    }
}
