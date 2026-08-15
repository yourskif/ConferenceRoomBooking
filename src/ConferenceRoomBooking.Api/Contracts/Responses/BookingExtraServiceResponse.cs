namespace ConferenceRoomBooking.Api.Contracts.Responses
{
    public class BookingExtraServiceResponse
    {
        public int ExtraServiceId { get; set; }

        public string ServiceName { get; set; } = string.Empty;

        public decimal Price { get; set; }
    }
}