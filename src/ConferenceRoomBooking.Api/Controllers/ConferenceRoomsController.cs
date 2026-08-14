using ConferenceRoomBooking.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConferenceRoomBooking.Api.Controllers
{
    [ApiController]
    [Route("api/conference-rooms")]
    public class ConferenceRoomsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConferenceRoomsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var conferenceRooms = await _context.ConferenceRooms
                .AsNoTracking()
                .Where(conferenceRoom => !conferenceRoom.IsDeleted)
                .Select(conferenceRoom => new
                {
                    conferenceRoom.Id,
                    conferenceRoom.Name,
                    conferenceRoom.Capacity,
                    conferenceRoom.BaseHourlyRate
                })
                .ToListAsync();

            return Ok(conferenceRooms);
        }
    }
}