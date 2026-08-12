using ConferenceRoomBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConferenceRoomBooking.Infrastructure.Persistence
{
    public static class SeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            var createdAt = new DateTime(2026, 8, 12, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<ConferenceRoom>().HasData(
                new ConferenceRoom
                {
                    Id = 1,
                    Name = "Зал А",
                    Capacity = 50,
                    BaseHourlyRate = 2000m,
                    IsDeleted = false,
                    CreatedAt = createdAt,
                    UpdatedAt = null
                },
                new ConferenceRoom
                {
                    Id = 2,
                    Name = "Зал B",
                    Capacity = 100,
                    BaseHourlyRate = 3500m,
                    IsDeleted = false,
                    CreatedAt = createdAt,
                    UpdatedAt = null
                },
                new ConferenceRoom
                {
                    Id = 3,
                    Name = "Зал C",
                    Capacity = 30,
                    BaseHourlyRate = 1500m,
                    IsDeleted = false,
                    CreatedAt = createdAt,
                    UpdatedAt = null
                }
            );

            modelBuilder.Entity<ExtraService>().HasData(
                new ExtraService
                {
                    Id = 1,
                    Name = "Проєктор",
                    Price = 500m,
                    CreatedAt = createdAt,
                    UpdatedAt = null
                },
                new ExtraService
                {
                    Id = 2,
                    Name = "Wi-Fi",
                    Price = 300m,
                    CreatedAt = createdAt,
                    UpdatedAt = null
                },
                new ExtraService
                {
                    Id = 3,
                    Name = "Звук",
                    Price = 700m,
                    CreatedAt = createdAt,
                    UpdatedAt = null
                }
            );

            modelBuilder.Entity<RoomExtraService>().HasData(
                new RoomExtraService
                {
                    ConferenceRoomId = 1,
                    ExtraServiceId = 1
                },
                new RoomExtraService
                {
                    ConferenceRoomId = 1,
                    ExtraServiceId = 2
                },
                new RoomExtraService
                {
                    ConferenceRoomId = 1,
                    ExtraServiceId = 3
                },
                new RoomExtraService
                {
                    ConferenceRoomId = 2,
                    ExtraServiceId = 1
                },
                new RoomExtraService
                {
                    ConferenceRoomId = 2,
                    ExtraServiceId = 2
                },
                new RoomExtraService
                {
                    ConferenceRoomId = 2,
                    ExtraServiceId = 3
                },
                new RoomExtraService
                {
                    ConferenceRoomId = 3,
                    ExtraServiceId = 1
                },
                new RoomExtraService
                {
                    ConferenceRoomId = 3,
                    ExtraServiceId = 2
                },
                new RoomExtraService
                {
                    ConferenceRoomId = 3,
                    ExtraServiceId = 3
                }
            );
        }
    }
}