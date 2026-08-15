namespace ConferenceRoomBooking.Application.Services
{
    public class PricingService
    {
        public decimal CalculateRoomPrice(
            decimal baseHourlyRate,
            DateTime startTime,
            DateTime endTime)
        {
            if (baseHourlyRate <= 0)
            {
                throw new ArgumentException("Base hourly rate must be greater than zero.");
            }

            if (startTime >= endTime)
            {
                throw new ArgumentException("Start time must be earlier than end time.");
            }

            var totalPrice = 0m;
            var currentTime = startTime;

            while (currentTime < endTime)
            {
                var nextBoundary = GetNextRateBoundary(currentTime);
                var periodEnd = nextBoundary < endTime ? nextBoundary : endTime;

                var hours = (decimal)(periodEnd - currentTime).TotalHours;
                var multiplier = GetHourlyRateMultiplier(TimeOnly.FromDateTime(currentTime));

                totalPrice += baseHourlyRate * multiplier * hours;

                currentTime = periodEnd;
            }

            return totalPrice;
        }

        private DateTime GetNextRateBoundary(DateTime currentTime)
        {
            var currentDate = currentTime.Date;
            var currentHour = TimeOnly.FromDateTime(currentTime);

            if (currentHour < new TimeOnly(9, 0))
            {
                return currentDate.AddHours(9);
            }

            if (currentHour < new TimeOnly(12, 0))
            {
                return currentDate.AddHours(12);
            }

            if (currentHour < new TimeOnly(14, 0))
            {
                return currentDate.AddHours(14);
            }

            if (currentHour < new TimeOnly(18, 0))
            {
                return currentDate.AddHours(18);
            }

            if (currentHour < new TimeOnly(23, 0))
            {
                return currentDate.AddHours(23);
            }

            throw new ArgumentException("Booking time must be between 06:00 and 23:00.");
        }

        private decimal GetHourlyRateMultiplier(TimeOnly time)
        {
            if (time >= new TimeOnly(6, 0) && time < new TimeOnly(9, 0))
            {
                return 0.9m;
            }

            if (time >= new TimeOnly(9, 0) && time < new TimeOnly(12, 0))
            {
                return 1m;
            }

            if (time >= new TimeOnly(12, 0) && time < new TimeOnly(14, 0))
            {
                return 1.15m;
            }

            if (time >= new TimeOnly(14, 0) && time < new TimeOnly(18, 0))
            {
                return 1m;
            }

            if (time >= new TimeOnly(18, 0) && time < new TimeOnly(23, 0))
            {
                return 0.8m;
            }

            throw new ArgumentException("Booking time must be between 06:00 and 23:00.");
        }
    }
}
