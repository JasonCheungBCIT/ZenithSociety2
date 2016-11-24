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
        public IEnumerable<Event> GetEvent()
        {
            return _context.Event.Include(@e => @e.Activity);
        }

        // GET: api/EventsAPI/5
        [Authorize]
        [HttpGet("{id}")]
        public IEnumerable<Event> GetEvent([FromRoute] int id)
        {
            //String[] blah = { "blah", "blue" };
            //return blah;
                
            return _context.Event.Include(@e => @e.Activity);
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //Event @event = await _context.Event.Include(@e => @e.Activity).SingleOrDefaultAsync(m => m.EventId == id) ;

            //if (@event == null)
            //{
            //    return NotFound();
            //}

            //return Ok(@event);
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