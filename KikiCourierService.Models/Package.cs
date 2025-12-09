
namespace KikiCourierService.Models
{
    public class Package
    {
        public string Id { get; init; } = null!;
        public double Weight { get; init; }
        public double Distance { get; init; }
        public string? OfferCode { get; init; }

        // Rich behavior - No more anemic model!
        public double CalculateFullCost(double baseCost) =>
            baseCost + (Weight * 10) + (Distance * 5);

        public bool IsEligibleFor(Offer offer) =>
            offer != null &&
            Distance >= offer.MinDistance && Distance <= offer.MaxDistance &&
            Weight >= offer.MinWeight && Weight <= offer.MaxWeight;

        public double CalculateDeliveryTime(double startTime, double speed) =>
            Math.Floor((startTime + Distance / speed) * 100) / 100.0;

        // Mutable state
        public double Discount { get; set; }
        public double TotalCost { get; set; }
        public double DeliveryTime { get; set; }

        public override string ToString() =>
            $"{Id} {(int)Discount} {(int)TotalCost} {DeliveryTime:F2}";
    }
}
