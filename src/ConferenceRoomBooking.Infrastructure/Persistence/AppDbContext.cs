using Microsoft.EntityFrameworkCore;
using ConferenceRoomBooking.Domain.Entities;

namespace ConferenceRoomBooking.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
        {
        }
        public DbSet<ConferenceRoom> ConferenceRooms { get; set; } = null!;
        public DbSet<ExtraService> ExtraServices { get; set; } = null!;
        public DbSet<RoomExtraService> RoomExtraServices { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<BookingExtraService> BookingExtraServices { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomExtraService>()
                .HasKey(roomExtraService => new
                {
                    roomExtraService.ConferenceRoomId,
                    roomExtraService.ExtraServiceId
                });

            modelBuilder.Entity<BookingExtraService>()
                .HasKey(bookingExtraService => new
                {
                    bookingExtraService.BookingId,
                    bookingExtraService.ExtraServiceId
                });

            modelBuilder.Entity<RoomExtraService>()
                .HasOne(roomExtraService => roomExtraService.ConferenceRoom)
                .WithMany(conferenceRoom => conferenceRoom.RoomExtraServices)
                .HasForeignKey(roomExtraService =>
            roomExtraService.ConferenceRoomId);

            modelBuilder.Entity<RoomExtraService>()
                .HasOne(roomExtraService => roomExtraService.ExtraService)
                .WithMany(extraService => extraService.RoomExtraServices)
                .HasForeignKey(roomExtraService =>
            roomExtraService.ExtraServiceId);

            modelBuilder.Entity<Booking>()
                .HasOne(booking => booking.ConferenceRoom)
                .WithMany(conferenceRoom => conferenceRoom.Bookings)
                .HasForeignKey(booking => booking.ConferenceRoomId);

            modelBuilder.Entity<BookingExtraService>()
                .HasOne(bookingExtraService => bookingExtraService.Booking)
                .WithMany(booking => booking.BookingExtraServices)
                .HasForeignKey(bookingExtraService =>
            bookingExtraService.BookingId);

            modelBuilder.Entity<BookingExtraService>()
                .HasOne(bookingExtraService =>
            bookingExtraService.ExtraService)
                .WithMany(extraService => extraService.BookingExtraServices)
                .HasForeignKey(bookingExtraService =>
            bookingExtraService.ExtraServiceId);

            SeedData.Seed(modelBuilder);

            base.OnModelCreating(modelBuilder);

        }
    }
}
