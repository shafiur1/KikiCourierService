

namespace KikiCourierService.Models
{
    public class Shipment
    {
        public List<Package> Packages { get; set; } = new();
        public int Count => Packages.Count;
        public double TotalWeight => Packages.Sum(p => p.Weight);
        public double MaxDist => Packages.Any() ? Packages.Max(p => p.Distance) : 0.0;
    }
}
