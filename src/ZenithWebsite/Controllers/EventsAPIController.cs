using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZenithWebsite.Data;
using ZenithWebsite.Models.ZenithSocietyModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace ZenithWebsite.Controllers
{

    [EnableCors("AllowAll")]
    [Produces("application/json")]
    [Route("api/EventsAPI")]
    public class EventsAPIController : Controller
    {
        private readonly ZenithContext _context;

        public EventsAPIController(ZenithContext context)
        {
            _context = context;
        }

        // GET: api/EventsAPI
        [HttpGet]
        public List<Event> GetEvent()
        {
            var @events = _context.Event.Include(that => that.Activity).OrderBy(s => s.FromDate);

            //Dictionary<String, List<Event>> Week = new Dictionary<String, List<Event>>();
            List<Event> Week = new List<Event>();
            //Find the monday of this week
            DateTime today = DateTime.Now;
            int delta = DayOfWeek.Monday - (today.DayOfWeek);
            if (delta > 0)
            {
                delta -= 7;
            }
                
            DateTime monday = today.AddDays(delta);
            DateTime sunday = monday.AddDays(6);

            foreach (var item in @events)
            {
                if (item.FromDate >= monday && item.FromDate <= sunday)
                {
                    if (item.IsActive)
                    {
                        Week.Add(item);
                    }

                }
            }
            
            return Week;
        }

        // GET: api/EventsAPI/5
        [Authorize]
        [HttpGet("{id}")]
        public IEnumerable<Event> GetEvent([FromRoute] int id)
        {
            var @events = _context.Event.Include(that => that.Activity).OrderBy(s => s.FromDate);

            //Dictionary<String, List<Event>> Week = new Dictionary<String, List<Event>>();
            List<Event> Week = new List<Event>();
            //Find the monday of this week
            int WeekOffset = (7 * id);
            DateTime today = DateTime.Now;
            //if (id != 0) {
            //    today = DateTime.Now.AddDays(WeekOffset);
            //}
            int delta = DayOfWeek.Monday - (today.DayOfWeek);
            if (delta > 0) { delta -= 7; }
                
            DateTime monday = today.AddDays(delta);
            DateTime sunday = monday.AddDays(7);

            foreach (var item in @events)
            {
                if (item.FromDate >= monday && item.FromDate < sunday)
                {
                    if (item.IsActive)
                    {
                        Week.Add(item);
                    }

                }
            }

            return Week;
        }

        // PUT: api/EventsAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent([FromRoute] int id, [FromBody] Event @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != @event.EventId)
            {
                return BadRequest();
            }

            _context.Entry(@event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/EventsAPI
        [HttpPost]
        public async Task<IActionResult> PostEvent([FromBody] Event @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Event.Add(@event);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EventExists(@event.EventId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEvent", new { id = @event.EventId }, @event);
        }

        // DELETE: api/EventsAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Event @event = await _context.Event.SingleOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            _context.Event.Remove(@event);
            await _context.SaveChangesAsync();

            return Ok(@event);
        }

        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.EventId == id);
        }
    }
}