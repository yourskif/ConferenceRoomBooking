using ConferenceRoomBooking.Api.Contracts.Responses;
using ConferenceRoomBooking.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConferenceRoomBooking.Api.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReportsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("revenue")]
        public async Task<IActionResult> GetRevenue()
        {
            var bookingPrices = await _context.Bookings
                .AsNoTracking()
                .Select(booking => booking.TotalPrice)
                .ToListAsync();

            var response = new RevenueReportResponse
            {
                TotalRevenue = bookingPrices.Sum(),
                BookingsCount = bookingPrices.Count
            };

            return Ok(response);
        }

        [HttpGet("room-utilization")]
        public async Task<IActionResult> GetRoomUtilization()
        {
            var bookings = await _context.Bookings
                .AsNoTracking()
                .Include(booking => booking.ConferenceRoom)
                .Select(booking => new
                {
                    booking.ConferenceRoomId,
                    ConferenceRoomName = booking.ConferenceRoom.Name,
                    booking.StartTime,
                    booking.EndTime
                })
                .ToListAsync();

            var response = bookings
                .GroupBy(booking => new
                {
                    booking.ConferenceRoomId,
                    booking.ConferenceRoomName
                })
                .Select(group => new RoomUtilizationReportResponse
                {
                    ConferenceRoomId = group.Key.ConferenceRoomId,
                    ConferenceRoomName = group.Key.ConferenceRoomName,
                    TotalBookedHours = group.Sum(booking =>
                        (decimal)(booking.EndTime - booking.StartTime).TotalHours),
                    BookingsCount = group.Count()
                })
                .OrderByDescending(report => report.TotalBookedHours)
                .ToList();

            return Ok(response);
        }

        [HttpGet("popular-services")]
        public async Task<IActionResult> GetPopularServices()
        {
            var bookingServices = await _context.BookingExtraServices
                .AsNoTracking()
                .Select(bookingExtraService => new
                {
                    bookingExtraService.ExtraServiceId,
                    bookingExtraService.ServiceName,
                    bookingExtraService.Price
                })
                .ToListAsync();

            var response = bookingServices
                .GroupBy(bookingService => new
                {
                    bookingService.ExtraServiceId,
                    bookingService.ServiceName
                })
                .Select(group => new PopularServiceReportResponse
                {
                    ExtraServiceId = group.Key.ExtraServiceId,
                    ServiceName = group.Key.ServiceName,
                    UsageCount = group.Count(),
                    TotalRevenue = group.Sum(bookingService => bookingService.Price)
                })
                .OrderByDescending(report => report.UsageCount)
                .ThenByDescending(report => report.TotalRevenue)
                .ToList();

            return Ok(response);
        }
    }
}
