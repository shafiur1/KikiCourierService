

namespace KikiCourierService.Models
{
    public class Offer
    {
        public string Code { get; init; } = null!;
        public double DiscountPercentage { get; init; }
        public double MinDistance { get; init; }
        public double MaxDistance { get; init; }
        public double MinWeight { get; init; }
        public double MaxWeight { get; init; }

        public double CalculateDiscountAmount(double fullCost) =>
            Math.Floor(fullCost * DiscountPercentage / 100);
    }
}
