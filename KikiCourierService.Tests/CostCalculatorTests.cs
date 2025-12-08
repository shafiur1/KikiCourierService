using FluentAssertions;
using KikiCourierService;
using KikiCourierService.Interfaces;
using KikiCourierService.Models;
using KikiCourierService.Services;
using Moq;

namespace KikiCourierService.Tests
{
    public class CostCalculatorTests
    {
        private readonly Mock<IDiscountService> _mockDiscountService;
        private readonly CostCalculator _calculator;
        private readonly List<Offer> _offers;

        public CostCalculatorTests()
        {
            _mockDiscountService = new Mock<IDiscountService>();
            _calculator = new CostCalculator(_mockDiscountService.Object);

            _offers = new HardcodedOfferRepository().GetAllOffers().ToList();
        }

        [Fact]
        public void CalculateCosts_Should_SetCorrectDiscountAndTotalCost_ForAllPackages()
        {
            // Arrange
            var packages = new List<Package>
        {
            new() { Id = "PKG1", Weight = 5,   Distance = 5,   OfferCode = "OFR001" },
            new() { Id = "PKG2", Weight = 15,  Distance = 5,   OfferCode = "OFR002" },
            new() { Id = "PKG3", Weight = 10,  Distance = 100, OfferCode = "OFR003" }
        };

            // Set up mock to return expected discounts
            _mockDiscountService
                .Setup(x => x.CalculateDiscount(packages[0], 175, _offers))
                .Returns(0);

            _mockDiscountService
                .Setup(x => x.CalculateDiscount(packages[1], 275, _offers))
                .Returns(0);

            _mockDiscountService
                .Setup(x => x.CalculateDiscount(packages[2], 700, _offers))
                .Returns(35);

            // Act
            _calculator.CalculateCosts(100, packages, _offers);

            // Assert
            packages[0].Discount.Should().Be(0);
            packages[0].TotalCost.Should().Be(175);

            packages[1].Discount.Should().Be(0);
            packages[1].TotalCost.Should().Be(275);

            packages[2].Discount.Should().Be(35);
            packages[2].TotalCost.Should().Be(665);
        }

        [Theory]
        [InlineData("OFR001", 100, 100, 700, 70)]   // 10% discount
        [InlineData("OFR001", 5, 5, 175, 0)]     // weight too low → no discount
        [InlineData("OFFR0008", 75, 125, 1475, 0)]   // invalid code → no discount
        [InlineData(null, 155, 95, 2125, 0)]         // no offer code
        public void CalculateCosts_Should_UseDiscountService_Correctly(
            string? offerCode, double weight, double distance, double expectedTotalCost, double expectedDiscount)
        {
            // Arrange
            var package = new Package { Id = "TEST", Weight = weight, Distance = distance, OfferCode = offerCode };
            var packages = new List<Package> { package };

            _mockDiscountService
                .Setup(x => x.CalculateDiscount(package, It.IsAny<double>(), _offers))
                .Returns(expectedDiscount);

            // Act
            _calculator.CalculateCosts(100, packages, _offers);

            // Assert
            package.Discount.Should().Be(expectedDiscount);
            package.TotalCost.Should().Be(expectedTotalCost);
            _mockDiscountService.Verify(x => x.CalculateDiscount(package, 100 + weight * 10 + distance * 5, _offers), Times.Once);
        }
    }
}