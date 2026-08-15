namespace ConferenceRoomBooking.Api.Contracts.Responses
{
    public class RoomUtilizationReportResponse
    {
        public int ConferenceRoomId { get; set; }

        public string ConferenceRoomName { get; set; } = string.Empty;

        public decimal TotalBookedHours { get; set; }

        public int BookingsCount { get; set; }
    }
}
