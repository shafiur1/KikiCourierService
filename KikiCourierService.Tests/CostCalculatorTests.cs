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
        private readonly IEnumerable<Offer> _offers;

        public CostCalculatorTests()
        {
            _mockDiscountService = new Mock<IDiscountService>();
            _calculator = new CostCalculator(_mockDiscountService.Object);

            // Use real offers from repository — this is important!
            _offers = new HardcodedOfferRepository().GetAllOffers();
        }

        [Fact]
        public void CalculateCosts_Should_SetCorrectDiscountAndTotalCost_ForOfficialSample()
        {
            // Arrange
            var packages = new List<Package>
        {
            new() { Id = "PKG1", Weight = 5,   Distance = 5,   OfferCode = "OFR001" },
            new() { Id = "PKG2", Weight = 15,  Distance = 5,   OfferCode = "OFR002" },
            new() { Id = "PKG3", Weight = 10,  Distance = 100, OfferCode = "OFR003" }
        };

            // Mock only the return values — do NOT mock the call signature with fullCost
            _mockDiscountService.Setup(x => x.CalculateDiscount(packages[0], It.IsAny<double>(), _offers)).Returns(0);
            _mockDiscountService.Setup(x => x.CalculateDiscount(packages[1], It.IsAny<double>(), _offers)).Returns(0);
            _mockDiscountService.Setup(x => x.CalculateDiscount(packages[2], It.IsAny<double>(), _offers)).Returns(35);

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
    }
}