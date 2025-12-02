using KikiCourierService.Interfaces;
using KikiCourierService.Models;


namespace KikiCourierService.Services
{
    public class CostCalculator : ICostCalculator
    {
        public void CalculateCosts(double baseCost, IEnumerable<Package> packages, IEnumerable<Offer> offers)
        {
            var offerDict = offers.ToDictionary(o => o.Code);
            foreach (var pkg in packages)
            {
                double cost = baseCost + pkg.Weight * 10 + pkg.Distance * 5;
                double discount = 0;
                if (!string.IsNullOrEmpty(pkg.OfferCode) && offerDict.TryGetValue(pkg.OfferCode, out var offer))
                {
                    if (pkg.Distance >= offer.MinDistance && pkg.Distance <= offer.MaxDistance &&
                        pkg.Weight >= offer.MinWeight && pkg.Weight <= offer.MaxWeight)
                    {
                        discount = cost * offer.DiscountPercentage / 100;
                    }
                }
                pkg.Discount = discount;
                pkg.TotalCost = cost - discount;
            }
        }
    }
}
