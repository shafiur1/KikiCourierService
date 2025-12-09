using KikiCourierService.Models;

namespace KikiCourierService.Interfaces
{
    public interface ICostCalculator
    {
        void CalculateCosts(double baseCost, List<Package> packages, IEnumerable<Offer> offers);
    }
}
