using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using visingsobiodlarna_backend.Data;
using visingsobiodlarna_backend.Models;

namespace visingsobiodlarna_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CalendarController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Hämtar alla händelser
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CalenderModel>>> GetCalendarEvents()
        {
            return await _context.CalenderModels.ToListAsync();
        }

        //Hämtar en händelse
        [HttpGet("{id}")]
        public async Task<ActionResult<CalenderModel>> GetCalendarEvent(int id)
        {
            var calendarEvent = await _context.CalenderModels.FindAsync(id);

            if (calendarEvent == null)
                return NotFound();

            return calendarEvent;
        }

        //Skapar en ny händelse
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CalenderModel>> CreateCalendarEvent(CalenderModel calendarEvent)
        {
            _context.CalenderModels.Add(calendarEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCalendarEvent), new { id = calendarEvent.Id }, calendarEvent);
        }

        //Uppdatera en händelse
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCalendarEvent(int id, CalenderModel calendarEvent)
        {
            if (id != calendarEvent.Id)
                return BadRequest();

            _context.Entry(calendarEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CalendarEventExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        //Radera en händelse
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCalendarEvent(int id)
        {
            var calendarEvent = await _context.CalenderModels.FindAsync(id);

            if (calendarEvent == null)
                return NotFound();

            _context.CalenderModels.Remove(calendarEvent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CalendarEventExists(int id)
        {
            return _context.CalenderModels.Any(e => e.Id == id);
        }
    }
}
