using KikiCourierService.Models;


namespace KikiCourierService.Interfaces
{
    public interface IDiscountService
    {
        double CalculateDiscount(Package package, double baseCost, IEnumerable<Offer> offers);
    }
}
