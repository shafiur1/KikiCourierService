using KikiCourierService.Interfaces;
using KikiCourierService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KikiCourierService.Services
{
    public class DiscountService : IDiscountService
    {
        public double CalculateDiscount(Package package, double baseCost, IEnumerable<Offer> offers)
        {
            if (string.IsNullOrWhiteSpace(package.OfferCode))
                return 0;

            var offer = offers.FirstOrDefault(o =>
                o.Code.Equals(package.OfferCode, StringComparison.OrdinalIgnoreCase));

            if (offer == null)
                return 0;

            bool isEligible =
                package.Distance >= offer.MinDistance && package.Distance <= offer.MaxDistance &&
                package.Weight >= offer.MinWeight && package.Weight <= offer.MaxWeight;

            return isEligible
                ? Math.Floor(baseCost * offer.DiscountPercentage / 100)
                : 0;
        }
    }
}
