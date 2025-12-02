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
        public void PlanDeliveries(List<Package> packages, int numVehicles, double speed, double maxCarriableWeight)
        {
            var vehiclePq = new PriorityQueue<Vehicle, double>();
            for (int i = 0; i < numVehicles; i++)
            {
                var v = new Vehicle();
                vehiclePq.Enqueue(v, v.AvailableTime);
            }

            var remaining = new List<Package>(packages);
            while (remaining.Any())
            {
                var vehicle = vehiclePq.Dequeue();
                double currentTime = vehicle.AvailableTime;

                var shipment = FindBestShipment(remaining, maxCarriableWeight);
                if (shipment.Count == 0) break;

                double maxDist = shipment.MaxDist;
                double oneWayTime = maxDist / speed;

                foreach (var p in shipment.Packages)
                {
                    p.DeliveryTime = Math.Round(currentTime + (p.Distance / speed), 2);
                    remaining.Remove(p);
                }

                vehicle.AvailableTime = Math.Round(currentTime + 2 * oneWayTime, 2);
                vehiclePq.Enqueue(vehicle, vehicle.AvailableTime);
            }
        }

        private Shipment FindBestShipment(List<Package> remaining, double maxWeight)
        {
            var best = new Shipment();
            var current = new List<Package>();
            Backtrack(0, current, 0.0, best, remaining, maxWeight);
            return best;
        }

        private void Backtrack(int index, List<Package> current, double currentSum, Shipment best, List<Package> remaining, double maxWeight)
        {
            if (index == remaining.Count)
            {
                double currTotalWeight = current.Sum(p => p.Weight);
                double currMaxDist = current.Any() ? current.Max(p => p.Distance) : 0.0;
                if (current.Count > best.Count ||
                    (current.Count == best.Count && currTotalWeight > best.TotalWeight) ||
                    (current.Count == best.Count && currTotalWeight == best.TotalWeight && currMaxDist < best.MaxDist))
                {
                    best.Packages = new List<Package>(current);
                }
                return;
            }

            // Skip this package
            Backtrack(index + 1, current, currentSum, best, remaining, maxWeight);

            // Include this package if possible
            var pkg = remaining[index];
            if (currentSum + pkg.Weight <= maxWeight)
            {
                current.Add(pkg);
                Backtrack(index + 1, current, currentSum + pkg.Weight, best, remaining, maxWeight);
                current.RemoveAt(current.Count - 1);
            }
        }
    }
}
