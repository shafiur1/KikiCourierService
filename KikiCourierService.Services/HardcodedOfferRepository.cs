using KikiCourierService.Interfaces;
using KikiCourierService.Models;


namespace KikiCourierService.Services
{
    public class HardcodedOfferRepository : IOfferRepository
    {
        public IEnumerable<Offer> GetAllOffers()
        {
            return new List<Offer>
        {
            new Offer { Code = "OFR001", DiscountPercentage = 10, MinDistance = 0, MaxDistance = 199, MinWeight = 70, MaxWeight = 200 },
            new Offer { Code = "OFR002", DiscountPercentage = 7, MinDistance = 50, MaxDistance = 150, MinWeight = 100, MaxWeight = 250 },
            new Offer { Code = "OFR003", DiscountPercentage = 5, MinDistance = 50, MaxDistance = 250, MinWeight = 10, MaxWeight = 150 }
        };
        }
    }
}
