using KikiCourierService.Models;
using KikiCourierService.Services;
using FluentAssertions;
namespace KikiCourierService.Tests
{
    public class DeliveryPlannerTests
    {
        [Fact]
        public void PlanDeliveries_ShouldMatchOfficialSample_Exactly()
        {
            var packages = new List<Package>
            {
                new() { Id = "PKG1", Weight = 50,  Distance = 30,  OfferCode = "OFR001" },
                new() { Id = "PKG2", Weight = 75,  Distance = 125, OfferCode = "OFFR0008" },
                new() { Id = "PKG3", Weight = 175, Distance = 100, OfferCode = "OFFR003" },
                new() { Id = "PKG4", Weight = 110, Distance = 60,  OfferCode = "OFFR002" },
                new() { Id = "PKG5", Weight = 155, Distance = 95,  OfferCode = "NA" }
            };

            var planner = new DeliveryPlanner();
            planner.PlanDeliveries(packages, 2, 70, 200);

            packages.First(p => p.Id == "PKG1").DeliveryTime.Should().Be(4.0);
            packages.First(p => p.Id == "PKG2").DeliveryTime.Should().Be(1.78);
            packages.First(p => p.Id == "PKG3").DeliveryTime.Should().Be(1.42);
            packages.First(p => p.Id == "PKG4").DeliveryTime.Should().Be(0.85);
            packages.First(p => p.Id == "PKG5").DeliveryTime.Should().Be(4.21);
        }
    }
}
