using KikiCourierService.Interfaces;
using KikiCourierService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KikiCourierService.Services
{
    public class DeliveryPlanner : IDeliveryPlanner
    {
        private static double FloorToTwoDecimals(double value) =>
            Math.Floor(value * 100) / 100.0;

        public void PlanDeliveries(List<Package> packages, int numVehicles, double speed, double maxCarriableWeight)
        {
            var vehicleQueue = new PriorityQueue<double, double>();
            for (int i = 0; i < numVehicles; i++)
                vehicleQueue.Enqueue(0.0, i);

            var remaining = new List<Package>(packages);

            while (remaining.Count > 0 && vehicleQueue.Count > 0)
            {
                double currentTime = vehicleQueue.Dequeue();

                var shipment = FindBestShipment(remaining, maxCarriableWeight);
                if (shipment.Count == 0) break;

                double maxDistance = shipment.Max(p => p.Distance);
                double roundTripTime = 2 * (maxDistance / speed);

                foreach (var pkg in shipment)
                {
                    pkg.DeliveryTime = FloorToTwoDecimals(currentTime + pkg.Distance / speed);
                    remaining.Remove(pkg);
                }

                vehicleQueue.Enqueue(currentTime + roundTripTime, currentTime + roundTripTime);
            }
        }

        private List<Package> FindBestShipment(List<Package> packages, double maxWeight)
        {
            var best = new List<Package>();

            void Backtrack(int index, List<Package> current, double weightSum)
            {
                if (index == packages.Count)
                {
                    if (IsBetter(current, best))
                        best = new List<Package>(current);
                    return;
                }

                Backtrack(index + 1, current, weightSum);

                if (weightSum + packages[index].Weight <= maxWeight)
                {
                    current.Add(packages[index]);
                    Backtrack(index + 1, current, weightSum + packages[index].Weight);
                    current.RemoveAt(current.Count - 1);
                }
            }

            bool IsBetter(IReadOnlyCollection<Package> a, IReadOnlyCollection<Package> b)
            {
                if (a.Count != b.Count) return a.Count > b.Count;
                double sumA = a.Sum(p => p.Weight);
                double sumB = b.Sum(p => p.Weight);
                if (Math.Abs(sumA - sumB) > 0.001) return sumA > sumB;

                double maxDistA = a.Count == 0 ? double.MaxValue : a.Max(p => p.Distance);
                double maxDistB = b.Count == 0 ? double.MaxValue : b.Max(p => p.Distance);
                return maxDistA < maxDistB;
            }

            Backtrack(0, new List<Package>(), 0.0);
            return best;
        }
    }
}
