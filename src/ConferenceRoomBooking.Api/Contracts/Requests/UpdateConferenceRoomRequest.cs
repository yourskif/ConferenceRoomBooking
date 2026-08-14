namespace ConferenceRoomBooking.Api.Contracts.Requests
{
    public class UpdateConferenceRoomRequest
    {
        public string Name { get; set; } = string.Empty;

        public int Capacity { get; set; }

        public decimal BaseHourlyRate { get; set; }

        public List<int> ServiceIds { get; set; } = [];
    }
}