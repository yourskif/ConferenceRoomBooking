using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ConferenceRoomBooking.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ConferenceRooms",
                columns: new[] { "Id", "BaseHourlyRate", "Capacity", "CreatedAt", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 2000m, 50, new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Utc), false, "Зал А", null },
                    { 2, 3500m, 100, new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Utc), false, "Зал B", null },
                    { 3, 1500m, 30, new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Utc), false, "Зал C", null }
                });

            migrationBuilder.InsertData(
                table: "ExtraServices",
                columns: new[] { "Id", "CreatedAt", "Name", "Price", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Utc), "Проєктор", 500m, null },
                    { 2, new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Utc), "Wi-Fi", 300m, null },
                    { 3, new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Utc), "Звук", 700m, null }
                });

            migrationBuilder.InsertData(
                table: "RoomExtraServices",
                columns: new[] { "ConferenceRoomId", "ExtraServiceId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 2, 1 },
                    { 2, 2 },
                    { 2, 3 },
                    { 3, 1 },
                    { 3, 2 },
                    { 3, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoomExtraServices",
                keyColumns: new[] { "ConferenceRoomId", "ExtraServiceId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "RoomExtraServices",
                keyColumns: new[] { "ConferenceRoomId", "ExtraServiceId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "RoomExtraServices",
                keyColumns: new[] { "ConferenceRoomId", "ExtraServiceId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "RoomExtraServices",
                keyColumns: new[] { "ConferenceRoomId", "ExtraServiceId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "RoomExtraServices",
                keyColumns: new[] { "ConferenceRoomId", "ExtraServiceId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "RoomExtraServices",
                keyColumns: new[] { "ConferenceRoomId", "ExtraServiceId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "RoomExtraServices",
                keyColumns: new[] { "ConferenceRoomId", "ExtraServiceId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "RoomExtraServices",
                keyColumns: new[] { "ConferenceRoomId", "ExtraServiceId" },
                keyValues: new object[] { 3, 2 });

            migrationBuilder.DeleteData(
                table: "RoomExtraServices",
                keyColumns: new[] { "ConferenceRoomId", "ExtraServiceId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ExtraServices",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ExtraServices",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ExtraServices",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
