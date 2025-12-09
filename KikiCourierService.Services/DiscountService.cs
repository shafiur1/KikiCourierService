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
            var offer = offers.FirstOrDefault(o =>
                string.Equals(o.Code, package.OfferCode, StringComparison.OrdinalIgnoreCase));

            return package.IsEligibleFor(offer)
                ? offer.CalculateDiscountAmount(package.CalculateFullCost(baseCost))
                : 0;
        }
    }
}
