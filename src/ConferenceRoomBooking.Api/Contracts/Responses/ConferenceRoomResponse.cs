namespace ConferenceRoomBooking.Api.Contracts.Responses
{
    public class ConferenceRoomResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Capacity { get; set; }

        public decimal BaseHourlyRate { get; set; }
    }
}