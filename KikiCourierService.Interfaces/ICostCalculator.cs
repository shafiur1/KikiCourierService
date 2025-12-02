using KikiCourierService.Models;

namespace KikiCourierService.Interfaces
{
    public interface ICostCalculator
    {
        void CalculateCosts(double baseCost, IEnumerable<Package> packages, IEnumerable<Offer> offers);
    }
}
