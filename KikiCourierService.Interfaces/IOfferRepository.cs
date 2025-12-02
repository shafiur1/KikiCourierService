using KikiCourierService.Models;


namespace KikiCourierService.Interfaces
{
    public interface IOfferRepository
    {
        IEnumerable<Offer> GetAllOffers();
    }
}
