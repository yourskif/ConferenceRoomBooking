using ConferenceRoomBooking.Api.Contracts.Requests;
using ConferenceRoomBooking.Api.Contracts.Responses;
using ConferenceRoomBooking.Application.Services;
using ConferenceRoomBooking.Domain.Entities;
using ConferenceRoomBooking.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConferenceRoomBooking.Api.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PricingService _pricingService;

        public BookingsController(
            AppDbContext context,
            PricingService pricingService)
        {
            _context = context;
            _pricingService = pricingService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingRequest request)
        {
            if (request.StartTime >= request.EndTime)
            {
                return BadRequest("Start time must be earlier than end time.");
            }

            var conferenceRoom = await _context.ConferenceRooms
                .Include(conferenceRoom => conferenceRoom.RoomExtraServices)
                .FirstOrDefaultAsync(conferenceRoom =>
                    conferenceRoom.Id == request.ConferenceRoomId &&
                    !conferenceRoom.IsDeleted);

            if (conferenceRoom is null)
            {
                return NotFound("Conference room was not found.");
            }

            var serviceIds = request.ServiceIds
                .Distinct()
                .ToList();

            var selectedServices = await _context.ExtraServices
                .Where(extraService => serviceIds.Contains(extraService.Id))
                .ToListAsync();

            if (selectedServices.Count != serviceIds.Count)
            {
                return BadRequest("One or more selected services do not exist.");
            }

            var availableServiceIds = conferenceRoom.RoomExtraServices
                .Select(roomExtraService => roomExtraService.ExtraServiceId)
                .ToHashSet();

            var hasUnavailableServices = serviceIds
                .Any(serviceId => !availableServiceIds.Contains(serviceId));

            if (hasUnavailableServices)
            {
                return BadRequest("One or more selected services are not available for this conference room.");
            }

            var hasOverlappingBooking = await _context.Bookings
                .AnyAsync(booking =>
                    booking.ConferenceRoomId == request.ConferenceRoomId &&
                    booking.StartTime < request.EndTime &&
                    request.StartTime < booking.EndTime);

            if (hasOverlappingBooking)
            {
                return Conflict("Conference room is not available for the selected time.");
            }

            decimal roomPrice;

            try
            {
                roomPrice = _pricingService.CalculateRoomPrice(
                    conferenceRoom.BaseHourlyRate,
                    request.StartTime,
                    request.EndTime);
            }
            catch (ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }

            var servicesPrice = selectedServices.Sum(extraService => extraService.Price);
            var totalPrice = roomPrice + servicesPrice;

            var booking = new Booking
            {
                ConferenceRoomId = conferenceRoom.Id,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                RoomPrice = roomPrice,
                ServicesPrice = servicesPrice,
                TotalPrice = totalPrice,
                CreatedAt = DateTime.UtcNow
            };

            foreach (var selectedService in selectedServices)
            {
                booking.BookingExtraServices.Add(new BookingExtraService
                {
                    ExtraServiceId = selectedService.Id,
                    ServiceName = selectedService.Name,
                    Price = selectedService.Price
                });
            }

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            var response = new BookingResponse
            {
                Id = booking.Id,
                ConferenceRoomId = booking.ConferenceRoomId,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                RoomPrice = booking.RoomPrice,
                ServicesPrice = booking.ServicesPrice,
                TotalPrice = booking.TotalPrice,
                Services = booking.BookingExtraServices
                    .Select(bookingExtraService => new BookingExtraServiceResponse
                    {
                        ExtraServiceId = bookingExtraService.ExtraServiceId,
                        ServiceName = bookingExtraService.ServiceName,
                        Price = bookingExtraService.Price
                    })
                    .ToList()
            };

            return Created($"/api/bookings/{booking.Id}", response);
        }
    }
}
