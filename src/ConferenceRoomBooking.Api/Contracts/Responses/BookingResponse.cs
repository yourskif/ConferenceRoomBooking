namespace ConferenceRoomBooking.Api.Contracts.Responses
{
    public class BookingResponse
    {
        public int Id { get; set; }

        public int ConferenceRoomId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public decimal RoomPrice { get; set; }

        public decimal ServicesPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public List<BookingExtraServiceResponse> Services { get; set; } = [];
    }
}