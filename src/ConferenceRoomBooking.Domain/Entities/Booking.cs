namespace ConferenceRoomBooking.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public int ConferenceRoomId { get; set; }
        public ConferenceRoom ConferenceRoom { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal RoomPrice { get; set; }
        public decimal ServicesPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<BookingExtraService> BookingExtraServices { get; set; } = [];
    }
}
