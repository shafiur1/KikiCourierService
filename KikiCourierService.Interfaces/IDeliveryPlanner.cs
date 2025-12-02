using KikiCourierService.Models;


namespace KikiCourierService.Interfaces
{
    public interface IDeliveryPlanner
    {
        void PlanDeliveries(List<Package> packages, int numVehicles, double speed, double maxCarriableWeight);
    }
}
