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
    }
}