namespace ConferenceRoomBooking.Api.Contracts.Responses
{
    public class PopularServiceReportResponse
    {
        public int ExtraServiceId { get; set; }

        public string ServiceName { get; set; } = string.Empty;

        public int UsageCount { get; set; }

        public decimal TotalRevenue { get; set; }
    }
}
