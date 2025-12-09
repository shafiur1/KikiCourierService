using KikiCourierService.Interfaces;
using KikiCourierService.Models;


namespace KikiCourierService.Services
{
    public class CostCalculator : ICostCalculator
    {
        private readonly IDiscountService _discountService;

        public CostCalculator(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        public void CalculateCosts(double baseCost, List<Package> packages, IEnumerable<Offer> offers)
        {
            foreach (var pkg in packages)
            {
                double fullCost = pkg.CalculateFullCost(baseCost);
                double discount = _discountService.CalculateDiscount(pkg, baseCost, offers);

                pkg.Discount = discount;
                pkg.TotalCost = fullCost - discount;
            }
        }
    }
}
