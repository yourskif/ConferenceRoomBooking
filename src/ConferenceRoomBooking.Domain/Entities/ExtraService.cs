namespace ConferenceRoomBooking.Domain.Entities
{
    public class ExtraService
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<RoomExtraService> RoomExtraServices { get; set; } = [];
        public ICollection<BookingExtraService> BookingExtraServices { get; set; } = [];
    }
}
