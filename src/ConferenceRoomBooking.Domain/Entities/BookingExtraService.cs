namespace ConferenceRoomBooking.Domain.Entities
{
    public class BookingExtraService
    {
        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;
        public int ExtraServiceId { get; set; }
        public ExtraService ExtraService { get; set; } = null!;
        public string ServiceName { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
