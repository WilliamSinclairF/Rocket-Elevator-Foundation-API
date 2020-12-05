using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rocket_REST_API.Models;

namespace Rocket_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly App _context;

        public CustomersController(App context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customers>>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customers>> GetCustomers(long id)
        {
            var customers = await _context.Customers.FindAsync(id);

            if (customers == null)
            {
                return NotFound();
            }

            return customers;
        }

        [HttpPut("update/{email}")]
        public async Task<IActionResult> UpdateCustomerInfo(string email, CustomerDTO customerDTO)
        {
            var decodedEmail = HttpUtility.UrlDecode(email);

            var user = await _context.Users.Where(u => u.Email == decodedEmail).SingleOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.Where(c => c.UserId == user.Id).SingleOrDefaultAsync();

            if (customer == null)
            {
                return NotFound();
            }

            if (customerDTO.CompanyName != null)
            {
                customer.CompanyName = customerDTO.CompanyName;
            }

            if (customerDTO.CompanyContactFullName != null)
            {
                customer.CompanyContactFullName = customerDTO.CompanyContactFullName;
            }
            if (customerDTO.CompanyContactPhone != null)
            {
                customer.CompanyContactPhone = customerDTO.CompanyContactPhone;
            }
            if (customerDTO.CompanyContactEmail != null)
            {
                customer.CompanyContactEmail = customerDTO.CompanyContactEmail;
            }
            if (customerDTO.CompanyDescription != null)
            {
                customer.CompanyDescription = customerDTO.CompanyDescription;
            }
            if (customerDTO.TechnicalAuthorityFullName != null)
            {
                customer.TechnicalAuthorityFullName = customerDTO.TechnicalAuthorityFullName;
            }
            if (customerDTO.TechnicalAuthorityPhoneNumber != null)
            {
                customer.TechnicalAuthorityPhoneNumber = customerDTO.TechnicalAuthorityPhoneNumber;
            }
            if (customerDTO.TechnicalManagerEmailService != null)
            {
                customer.TechnicalManagerEmailService = customerDTO.TechnicalManagerEmailService;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomersExists(customer.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(customerDTO);
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomers(long id, Customers customers)
        {
            if (id != customers.Id)
            {
                return BadRequest();
            }

            _context.Entry(customers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomersExists(id))
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

        // POST: api/Customers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Customers>> PostCustomers(Customers customers)
        {
            _context.Customers.Add(customers);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomers", new { id = customers.Id }, customers);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customers>> DeleteCustomers(long id)
        {
            var customers = await _context.Customers.FindAsync(id);
            if (customers == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customers);
            await _context.SaveChangesAsync();

            return customers;
        }

        private bool CustomersExists(long id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
