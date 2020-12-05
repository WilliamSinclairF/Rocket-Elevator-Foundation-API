using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rocket_REST_API;
using Rocket_REST_API.Models;

namespace Rocket_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterventionsController : ControllerBase
    {
        private const string Pending = "Pending";
        private const string InProgress = "InProgress";
        private const string Complete = "Completed";

        private readonly App _context;

        public InterventionsController(App context)
        {
            _context = context;
        }

        // GET: api/Interventions/pending
        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<Interventions>>> GetPendingInterventions()
        {
            return await _context.Interventions.Where(e => e.Status == Pending && e.InterventionStartDateTime == null).ToListAsync();
        }

        // GET: api/Interventions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Interventions>>> GetInterventions()
        {
            return await _context.Interventions.ToListAsync();
        }

        // GET: api/Interventions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Interventions>> GetInterventions(long id)
        {
            var interventions = await _context.Interventions.FindAsync(id);

            if (interventions == null)
            {
                return NotFound();
            }

            return interventions;

        }


        // Change the status of an intervention to in progress
        // PUT: api/Interventions/set_in_progress/5
        [HttpPut("set_in_progress/{id}")]
        public async Task<IActionResult> SetInterventionStatusInProgress(long id)
        {
            var intervention = await _context.Interventions.FindAsync(id);
            if (intervention == null)
            {
                return NotFound();
            }

            intervention.Status = InProgress;
            intervention.InterventionStartDateTime = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InterventionsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(id);
        }

        // Change the status of an intervention to complete
        // PUT: api/Interventions/set_complete/5
        [HttpPut("set_complete/{id}")]
        public async Task<IActionResult> SetInterventionStatusComplete(long id)
        {
            var intervention = await _context.Interventions.FindAsync(id);
            if (intervention == null)
            {
                return NotFound();
            }

            intervention.Status = Complete;
            intervention.InterventionEndDateTime = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InterventionsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(id);
        }


        [HttpPost("portal_intervention")]
        public async Task<ActionResult<CustomerPortalInterventionDTO>> CreateCustomerPortalInvervention(CustomerPortalInterventionDTO customerPortalInterventionsDTO)
        {
            Users user = await _context.Users.Where(u => u.Email == customerPortalInterventionsDTO.CustomerEmail).SingleAsync();

            if (user == null)
            {
                return Unauthorized();
            }

            Customers customer = await _context.Customers.Where(c => c.UserId == user.Id).SingleAsync();

            var intervention = new Interventions
            {
                CustomerId = (int)customer.Id,
                AuthorId = (int)customer.Id,
                BuildingId = (int)customerPortalInterventionsDTO.BuildingId,
                CreatedAt = DateTime.UtcNow,
                EmployeeId = 1377,
                Status = "Pending"
            };


            if (customerPortalInterventionsDTO.BatteryId != null)
            {
                intervention.BatteryId = (int)customerPortalInterventionsDTO.BatteryId;
            }
            if (customerPortalInterventionsDTO.ColumnId != null)
            {
                intervention.ColumnId = (int)customerPortalInterventionsDTO.ColumnId;
            }
            if (customerPortalInterventionsDTO.ElevatorId != null)
            {
                intervention.ElevatorId = (int)customerPortalInterventionsDTO.ElevatorId;
            }
            if (customerPortalInterventionsDTO.Report != null)
            {
                intervention.Report = customerPortalInterventionsDTO.Report;
            }

            intervention.CustomerId = (int)customer.Id;
            intervention.AuthorId = (int)customer.Id;

            _context.Interventions.Add(intervention);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInterventions", new { id = intervention.Id }, intervention);
        }

        // PUT: api/Interventions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInterventions(long id, Interventions interventions)
        {
            if (id != interventions.Id)
            {
                return BadRequest();
            }

            _context.Entry(interventions).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InterventionsExists(id))
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

        // POST: api/Interventions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Interventions>> PostInterventions(Interventions interventions)
        {
            _context.Interventions.Add(interventions);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInterventions", new { id = interventions.Id }, interventions);
        }

        // DELETE: api/Interventions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Interventions>> DeleteInterventions(long id)
        {
            var interventions = await _context.Interventions.FindAsync(id);
            if (interventions == null)
            {
                return NotFound();
            }

            _context.Interventions.Remove(interventions);
            await _context.SaveChangesAsync();

            return interventions;
        }

        private bool InterventionsExists(long id)
        {
            return _context.Interventions.Any(e => e.Id == id);
        }
    }
}
