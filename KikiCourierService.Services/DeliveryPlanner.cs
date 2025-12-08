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
        // This is the exact rounding used in the official PDF sample
        private static double RoundToTwoDecimals(double value)
        {
            // They use banker's rounding but floor for time (125/70 = 1.7857 → 1.78)
            return Math.Floor(value * 100) / 100.0;
        }

        public void PlanDeliveries(List<Package> packages, int numVehicles, double speed, double maxCarriableWeight)
        {
            var availableTimes = new PriorityQueue<double, double>();
            for (int i = 0; i < numVehicles; i++)
                availableTimes.Enqueue(0.0, i);

            var remaining = new List<Package>(packages);

            while (remaining.Count > 0 && availableTimes.Count > 0)
            {
                double currentTime = availableTimes.Dequeue();

                var shipment = FindBestShipment(remaining, maxCarriableWeight);
                if (shipment.Count == 0) break;

                double maxDist = shipment.Max(p => p.Distance);
                double oneWayTime = maxDist / speed;
                double roundTripTime = 2 * oneWayTime;

                foreach (var pkg in shipment)
                {
                    double travelTime = pkg.Distance / speed;
                    pkg.DeliveryTime = RoundToTwoDecimals(currentTime + travelTime);
                    remaining.Remove(pkg);
                }

                double nextAvailableTime = currentTime + roundTripTime;
                availableTimes.Enqueue(nextAvailableTime, nextAvailableTime);
            }
        }

        private List<Package> FindBestShipment(List<Package> packages, double maxWeight)
        {
            var best = new List<Package>();

            void Backtrack(int index, List<Package> current, double currentWeight)
            {
                if (index == packages.Count)
                {
                    if (IsBetter(current, best))
                        best = new List<Package>(current);
                    return;
                }

                // Skip
                Backtrack(index + 1, current, currentWeight);

                // Include if possible
                if (currentWeight + packages[index].Weight <= maxWeight)
                {
                    current.Add(packages[index]);
                    Backtrack(index + 1, current, currentWeight + packages[index].Weight);
                    current.RemoveAt(current.Count - 1);
                }
            }

            bool IsBetter(IReadOnlyCollection<Package> a, IReadOnlyCollection<Package> b)
            {
                if (a.Count != b.Count) return a.Count > b.Count;
                double weightA = a.Sum(p => p.Weight);
                double weightB = b.Sum(p => p.Weight);
                if (Math.Abs(weightA - weightB) > 0.001) return weightA > weightB;

                double maxDistA = a.Count == 0 ? double.MaxValue : a.Max(p => p.Distance);
                double maxDistB = b.Count == 0 ? double.MaxValue : b.Max(p => p.Distance);
                return maxDistA < maxDistB;
            }

            Backtrack(0, new List<Package>(), 0.0);
            return best;
        }
    }
}
