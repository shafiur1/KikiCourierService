using KikiCourierService.Models;
using KikiCourierService.Services;
using FluentAssertions;

namespace KikiCourierService.Tests
{
    public class DiscountServiceTests
    {
        private readonly DiscountService _service;
        private readonly List<Offer> _offers;

        public DiscountServiceTests()
        {
            _offers = new()
        {
            new Offer { Code = "OFR001", DiscountPercentage = 10, MinDistance = 0,   MaxDistance = 199, MinWeight = 70, MaxWeight = 200 },
            new Offer { Code = "OFR002", DiscountPercentage = 7,  MinDistance = 50,  MaxDistance = 150, MinWeight = 100, MaxWeight = 250 },
            new Offer { Code = "OFR003", DiscountPercentage = 5,  MinDistance = 50,  MaxDistance = 250, MinWeight = 10,  MaxWeight = 150 }
        };
            _service = new DiscountService();
        }

        [Theory]
        [InlineData("OFR001", 100, 100, 700, 70)]   // 10% of 700
        [InlineData("OFR001", 5, 5, 175, 0)]     // weight too low
        [InlineData("OFR001", 100, 250, 1750, 0)]   // distance too high
        [InlineData("OFR002", 125, 100, 1475, 103)]  // 7% of 1475 ≈ 103.25 → 103
        [InlineData("OFR003", 100, 100, 700, 35)]   // 5% of 700
        [InlineData("OFFR0008", 125, 125, 1600, 0)]  // invalid code
        [InlineData(null, 100, 100, 700, 0)]         // no offer
        public void CalculateDiscount_ShouldReturnCorrectValue(
            string? offerCode, double distance, double weight, double fullCost, double expectedDiscount)
        {
            // Arrange
            var package = new Package { OfferCode = offerCode, Distance = distance, Weight = weight };

            // Act
            double discount = _service.CalculateDiscount(package, fullCost, _offers);

            // Assert
            discount.Should().Be(expectedDiscount);
        }
    }
}
