using KikiCourierService.Models;
using KikiCourierService.Services;
using FluentAssertions;
namespace KikiCourierService.Tests
{
    public class DeliveryPlannerTests
    {
        [Fact]
        public void PlanDeliveries_ShouldMatchOfficialSampleExactly()
        {
            // Arrange - Official sample input
            var packages = new List<Package>
        {
            new() { Id = "PKG1", Weight = 50,  Distance = 30,  OfferCode = "OFR001" },
            new() { Id = "PKG2", Weight = 75,  Distance = 125, OfferCode = "OFFR0008" },
            new() { Id = "PKG3", Weight = 175, Distance = 100, OfferCode = "OFFR003" },
            new() { Id = "PKG4", Weight = 110, Distance = 60,  OfferCode = "OFFR002" },
            new() { Id = "PKG5", Weight = 155, Distance = 95,  OfferCode = "NA" }
        };

            var planner = new DeliveryPlanner();
            const int numVehicles = 2;
            const double speed = 70;
            const double maxWeight = 200;

            // Act
            planner.PlanDeliveries(packages, numVehicles, speed, maxWeight);

            // Assert - Must match sample output exactly
            packages.Should().Contain(p => p.Id == "PKG1" && Math.Abs(p.DeliveryTime - 3.98) < 0.01);
            packages.Should().Contain(p => p.Id == "PKG2" && Math.Abs(p.DeliveryTime - 1.78) < 0.01);
            packages.Should().Contain(p => p.Id == "PKG3" && Math.Abs(p.DeliveryTime - 1.42) < 0.01);
            packages.Should().Contain(p => p.Id == "PKG4" && Math.Abs(p.DeliveryTime - 0.85) < 0.01);
            packages.Should().Contain(p => p.Id == "PKG5" && Math.Abs(p.DeliveryTime - 4.19) < 0.01);
        }

        [Fact]
        public void PlanDeliveries_ShouldPreferMaxPackages_ThenHeavier_ThenSoonerDelivery()
        {
            var packages = new List<Package>
        {
            new() { Id = "A", Weight = 50, Distance = 30 },
            new() { Id = "B", Weight = 50, Distance = 30 },
            new() { Id = "C", Weight = 150, Distance = 100 }
        };

            var planner = new DeliveryPlanner();
            planner.PlanDeliveries(packages, 1, 70, 200);

            // Should pick A+B (2 packages) over just C (1 package)
            packages.Count(p => p.DeliveryTime > 0).Should().Be(2);
        }
    }
}
