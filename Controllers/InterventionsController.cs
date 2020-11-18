﻿using System;
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
        private const string pending = "0";
        private const string inProgress = "1";
        private const string resultComplete = "2";

        private readonly App _context;

        public InterventionsController(App context)
        {
            _context = context;
        }

        // GET: api/Interventions/pending
        [HttpGet ("pending")]
        public async Task<ActionResult<IEnumerable<Interventions>>> GetPendingInterventions()
        {
            return await _context.Interventions.Where(e => e.Status == pending && e.InterventionStartDateTime == null).ToListAsync();
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

            intervention.Status = inProgress;
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

            intervention.Status = resultComplete;
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