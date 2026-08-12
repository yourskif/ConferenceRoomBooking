namespace ConferenceRoomBooking.Domain.Entities
{
    public class RoomExtraService
    {
        public int ConferenceRoomId { get; set; }
        public ConferenceRoom ConferenceRoom { get; set; } = null!;
        public int ExtraServiceId { get; set; }
        public ExtraService ExtraService { get; set; } = null!;
    }
}
