using ConferenceRoomBooking.Application.Services;

namespace ConferenceRoomBooking.Tests.Services
{
    public class PricingServiceTests
    {
        [Fact]
        public void CalculateRoomPrice_WhenBookingIsInStandardMorningPeriod_ReturnsBasePrice()
        {
            var pricingService = new PricingService();

            var startTime = new DateTime(2026, 8, 20, 10, 0, 0);
            var endTime = new DateTime(2026, 8, 20, 12, 0, 0);

            var price = pricingService.CalculateRoomPrice(
                baseHourlyRate: 2000m,
                startTime: startTime,
                endTime: endTime);

            Assert.Equal(4000m, price);
        }
    }
}