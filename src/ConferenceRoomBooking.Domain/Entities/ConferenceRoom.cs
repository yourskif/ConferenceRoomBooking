namespace ConferenceRoomBooking.Domain.Entities
{
    public class ConferenceRoom
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public decimal BaseHourlyRate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<RoomExtraService> RoomExtraServices { get; set; } = [];
        public ICollection<Booking> Bookings { get; set; } = [];
    }
}
