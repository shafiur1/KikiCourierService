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

        public void CalculateCosts(double baseCost, IEnumerable<Package> packages, IEnumerable<Offer> offers)
        {
            foreach (var pkg in packages)
            {
                double fullCost = baseCost + (pkg.Weight * 10) + (pkg.Distance * 5);
                double discount = _discountService.CalculateDiscount(pkg, fullCost, offers);

                pkg.Discount = discount;
                pkg.TotalCost = fullCost - discount;
            }
        }
    }
}
