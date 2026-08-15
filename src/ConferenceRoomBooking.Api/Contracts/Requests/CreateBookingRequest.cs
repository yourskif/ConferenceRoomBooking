namespace ConferenceRoomBooking.Api.Contracts.Requests
{
    public class CreateBookingRequest
    {
        public int ConferenceRoomId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public List<int> ServiceIds { get; set; } = [];
    }
}