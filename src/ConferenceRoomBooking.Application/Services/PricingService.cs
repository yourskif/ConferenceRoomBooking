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

            var duration = endTime - startTime;
            var hours = (decimal)duration.TotalHours;

            return baseHourlyRate * hours;
        }
    }
}