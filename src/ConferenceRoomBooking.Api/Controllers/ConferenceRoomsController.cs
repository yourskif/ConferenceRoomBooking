using ConferenceRoomBooking.Domain.Entities;
using ConferenceRoomBooking.Api.Contracts.Requests;
using ConferenceRoomBooking.Api.Contracts.Responses;
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
                .Select(conferenceRoom => new ConferenceRoomResponse
                {
                    Id = conferenceRoom.Id,
                    Name = conferenceRoom.Name,
                    Capacity = conferenceRoom.Capacity,
                    BaseHourlyRate = conferenceRoom.BaseHourlyRate
                })
                .ToListAsync();

            return Ok(conferenceRooms);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var conferenceRoom = await _context.ConferenceRooms
                .AsNoTracking()
                .Where(conferenceRoom => !conferenceRoom.IsDeleted)
                .Where(conferenceRoom => conferenceRoom.Id == id)
                .Select(conferenceRoom => new ConferenceRoomResponse
                {
                    Id = conferenceRoom.Id,
                    Name = conferenceRoom.Name,
                    Capacity = conferenceRoom.Capacity,
                    BaseHourlyRate = conferenceRoom.BaseHourlyRate
                })
                .FirstOrDefaultAsync();

            if (conferenceRoom is null)
            {
                return NotFound();
            }

            return Ok(conferenceRoom);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateConferenceRoomRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Conference room name is required.");
            }

            if (request.Capacity <= 0)
            {
                return BadRequest("Conference room capacity must be greater than zero.");
            }

            if (request.BaseHourlyRate <= 0)
            {
                return BadRequest("Base hourly rate must be greater than zero.");
            }

            var serviceIds = request.ServiceIds
                .Distinct()
                .ToList();

            var existingServiceIds = await _context.ExtraServices
                .Where(extraService => serviceIds.Contains(extraService.Id))
                .Select(extraService => extraService.Id)
                .ToListAsync();

            if (existingServiceIds.Count != serviceIds.Count)
            {
                return BadRequest("One or more selected services do not exist.");
            }

            var conferenceRoom = new ConferenceRoom
            {
                Name = request.Name.Trim(),
                Capacity = request.Capacity,
                BaseHourlyRate = request.BaseHourlyRate,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            };

            foreach (var serviceId in serviceIds)
            {
                conferenceRoom.RoomExtraServices.Add(new RoomExtraService
                {
                    ExtraServiceId = serviceId
                });
            }

            _context.ConferenceRooms.Add(conferenceRoom);
            await _context.SaveChangesAsync();

            var response = new ConferenceRoomResponse
            {
                Id = conferenceRoom.Id,
                Name = conferenceRoom.Name,
                Capacity = conferenceRoom.Capacity,
                BaseHourlyRate = conferenceRoom.BaseHourlyRate
            };

            return CreatedAtAction(nameof(GetById), new { id = conferenceRoom.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateConferenceRoomRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Conference room name is required.");
            }

            if (request.Capacity <= 0)
            {
                return BadRequest("Conference room capacity must be greater than zero.");
            }

            if (request.BaseHourlyRate <= 0)
            {
                return BadRequest("Base hourly rate must be greater than zero.");
            }

            var conferenceRoom = await _context.ConferenceRooms
                .Include(conferenceRoom => conferenceRoom.RoomExtraServices)
                .FirstOrDefaultAsync(conferenceRoom =>
                    conferenceRoom.Id == id && !conferenceRoom.IsDeleted);

            if (conferenceRoom is null)
            {
                return NotFound();
            }

            var serviceIds = request.ServiceIds
                .Distinct()
                .ToList();

            var existingServiceIds = await _context.ExtraServices
                .Where(extraService => serviceIds.Contains(extraService.Id))
                .Select(extraService => extraService.Id)
                .ToListAsync();

            if (existingServiceIds.Count != serviceIds.Count)
            {
                return BadRequest("One or more selected services do not exist.");
            }

            conferenceRoom.Name = request.Name.Trim();
            conferenceRoom.Capacity = request.Capacity;
            conferenceRoom.BaseHourlyRate = request.BaseHourlyRate;
            conferenceRoom.UpdatedAt = DateTime.UtcNow;

            _context.RoomExtraServices.RemoveRange(conferenceRoom.RoomExtraServices);

            conferenceRoom.RoomExtraServices = serviceIds
                .Select(serviceId => new RoomExtraService
                {
                    ConferenceRoomId = conferenceRoom.Id,
                    ExtraServiceId = serviceId
                })
                .ToList();

            await _context.SaveChangesAsync();

            var response = new ConferenceRoomResponse
            {
                Id = conferenceRoom.Id,
                Name = conferenceRoom.Name,
                Capacity = conferenceRoom.Capacity,
                BaseHourlyRate = conferenceRoom.BaseHourlyRate
            };

            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var conferenceRoom = await _context.ConferenceRooms
                .FirstOrDefaultAsync(conferenceRoom =>
                    conferenceRoom.Id == id && !conferenceRoom.IsDeleted);

            if (conferenceRoom is null)
            {
                return NotFound();
            }

            conferenceRoom.IsDeleted = true;
            conferenceRoom.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}