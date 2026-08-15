namespace ConferenceRoomBooking.Api.Contracts.Requests
{
    public class AvailableConferenceRoomsRequest
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int Capacity { get; set; }
    }
}