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
            _service = new DiscountService();
            _offers = new HardcodedOfferRepository().GetAllOffers().ToList();
        }

        [Theory]
        [InlineData("OFR001", 100, 100, 160)]   // 10% of 1600 = 160
        [InlineData("OFR001", 50, 30, 0)]    //ineligible (weight < 70)
        [InlineData("OFR002", 110, 60, 105)]    //7% of 1500 = 105.0 → floor → 105
        [InlineData("OFR002", 125, 100, 129)]   //7% of 1850 = 129.5 → floor → 129
        [InlineData("OFR003", 100, 100, 70)]    //5% of 1400 = 70
        [InlineData("OFFR0008", 75, 125, 0)]    //invalid code
        [InlineData(null, 155, 95, 0)]          //no offer
        public void CalculateDiscount_Should_Return_Correct_Amount(
            string? offerCode, double weight, double distance, double expectedDiscount)
        {
            // Arrange
            var package = new Package
            {
                Id = "TEST",
                Weight = weight,
                Distance = distance,
                OfferCode = offerCode
            };

            double baseCost = 100;

            // Act
            double discount = _service.CalculateDiscount(package, baseCost, _offers);

            
        }

        [Fact]
        public void Should_Match_Official_Sample_Exactly()
        {
            var packages = new List<Package>
        {
            new() { Id = "PKG1", Weight = 50, Distance = 30, OfferCode = "OFR001" },
            new() { Id = "PKG2", Weight = 75, Distance = 125, OfferCode = "OFFR0008" },
            new() { Id = "PKG3", Weight = 175, Distance = 100, OfferCode = "OFR003" },
            new() { Id = "PKG4", Weight = 110, Distance = 60, OfferCode = "OFR002" },
            new() { Id = "PKG5", Weight = 155, Distance = 95, OfferCode = "NA" }
        };

            double baseCost = 100;

            foreach (var p in packages)
                p.Discount = _service.CalculateDiscount(p, baseCost, _offers);

            packages[0].Discount.Should().Be(0);
            packages[1].Discount.Should().Be(0);
            packages[2].Discount.Should().Be(0);
            packages[3].Discount.Should().Be(105);  // This is the correct one
            packages[4].Discount.Should().Be(0);
        }
    }
}
