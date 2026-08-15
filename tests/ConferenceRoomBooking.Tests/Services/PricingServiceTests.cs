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

        [Fact]
        public void CalculateRoomPrice_WhenBookingIsInMorningDiscountPeriod_ReturnsDiscountedPrice()
        {
            var pricingService = new PricingService();

            var startTime = new DateTime(2026, 8, 20, 6, 0, 0);
            var endTime = new DateTime(2026, 8, 20, 9, 0, 0);

            var price = pricingService.CalculateRoomPrice(
                baseHourlyRate: 1000m,
                startTime: startTime,
                endTime: endTime);

            Assert.Equal(2700m, price);
        }

        [Fact]
        public void CalculateRoomPrice_WhenBookingIsInPeakPeriod_ReturnsIncreasedPrice()
        {
            var pricingService = new PricingService();

            var startTime = new DateTime(2026, 8, 20, 12, 0, 0);
            var endTime = new DateTime(2026, 8, 20, 14, 0, 0);

            var price = pricingService.CalculateRoomPrice(
                baseHourlyRate: 1000m,
                startTime: startTime,
                endTime: endTime);

            Assert.Equal(2300m, price);
        }

        [Fact]
        public void CalculateRoomPrice_WhenBookingIsInStandardAfternoonPeriod_ReturnsBasePrice()
        {
            var pricingService = new PricingService();

            var startTime = new DateTime(2026, 8, 20, 14, 0, 0);
            var endTime = new DateTime(2026, 8, 20, 18, 0, 0);

            var price = pricingService.CalculateRoomPrice(
                baseHourlyRate: 1000m,
                startTime: startTime,
                endTime: endTime);

            Assert.Equal(4000m, price);
        }

        [Fact]
        public void CalculateRoomPrice_WhenBookingIsInEveningDiscountPeriod_ReturnsDiscountedPrice()
        {
            var pricingService = new PricingService();

            var startTime = new DateTime(2026, 8, 20, 18, 0, 0);
            var endTime = new DateTime(2026, 8, 20, 23, 0, 0);

            var price = pricingService.CalculateRoomPrice(
                baseHourlyRate: 1000m,
                startTime: startTime,
                endTime: endTime);

            Assert.Equal(4000m, price);
        }

        [Fact]
        public void CalculateRoomPrice_WhenBookingCrossesStandardAndPeakPeriods_ReturnsCombinedPrice()
        {
            var pricingService = new PricingService();

            var startTime = new DateTime(2026, 8, 20, 10, 0, 0);
            var endTime = new DateTime(2026, 8, 20, 13, 0, 0);

            var price = pricingService.CalculateRoomPrice(
                baseHourlyRate: 1000m,
                startTime: startTime,
                endTime: endTime);

            Assert.Equal(3150m, price);
        }

        [Fact]
        public void CalculateRoomPrice_WhenBookingCrossesMorningDiscountAndStandardPeriods_ReturnsCombinedPrice()
        {
            var pricingService = new PricingService();

            var startTime = new DateTime(2026, 8, 20, 8, 0, 0);
            var endTime = new DateTime(2026, 8, 20, 10, 0, 0);

            var price = pricingService.CalculateRoomPrice(
                baseHourlyRate: 1000m,
                startTime: startTime,
                endTime: endTime);

            Assert.Equal(1900m, price);
        }

        [Fact]
        public void CalculateRoomPrice_WhenBaseHourlyRateIsZero_ThrowsArgumentException()
        {
            var pricingService = new PricingService();

            var startTime = new DateTime(2026, 8, 20, 10, 0, 0);
            var endTime = new DateTime(2026, 8, 20, 12, 0, 0);

            var exception = Assert.Throws<ArgumentException>(() =>
                pricingService.CalculateRoomPrice(
                    baseHourlyRate: 0m,
                    startTime: startTime,
                    endTime: endTime));

            Assert.Equal("Base hourly rate must be greater than zero.", exception.Message);
        }

        [Fact]
        public void CalculateRoomPrice_WhenStartTimeIsAfterEndTime_ThrowsArgumentException()
        {
            var pricingService = new PricingService();

            var startTime = new DateTime(2026, 8, 20, 12, 0, 0);
            var endTime = new DateTime(2026, 8, 20, 10, 0, 0);

            var exception = Assert.Throws<ArgumentException>(() =>
                pricingService.CalculateRoomPrice(
                    baseHourlyRate: 1000m,
                    startTime: startTime,
                    endTime: endTime));

            Assert.Equal("Start time must be earlier than end time.", exception.Message);
        }

        [Fact]
        public void CalculateRoomPrice_WhenBookingStartsBeforeWorkingHours_ThrowsArgumentException()
        {
            var pricingService = new PricingService();

            var startTime = new DateTime(2026, 8, 20, 5, 0, 0);
            var endTime = new DateTime(2026, 8, 20, 6, 0, 0);

            var exception = Assert.Throws<ArgumentException>(() =>
                pricingService.CalculateRoomPrice(
                    baseHourlyRate: 1000m,
                    startTime: startTime,
                    endTime: endTime));

            Assert.Equal("Booking time must be between 06:00 and 23:00.", exception.Message);
        }

        [Fact]
        public void CalculateRoomPrice_WhenBookingEndsAfterWorkingHours_ThrowsArgumentException()
        {
            var pricingService = new PricingService();

            var startTime = new DateTime(2026, 8, 20, 22, 0, 0);
            var endTime = new DateTime(2026, 8, 21, 0, 0, 0);

            var exception = Assert.Throws<ArgumentException>(() =>
                pricingService.CalculateRoomPrice(
                    baseHourlyRate: 1000m,
                    startTime: startTime,
                    endTime: endTime));

            Assert.Equal("Booking time must be between 06:00 and 23:00.", exception.Message);
        }
    }
}