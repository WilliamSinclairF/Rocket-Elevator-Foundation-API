using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rocket_REST_API.Models;

namespace Rocket_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly App _context;

        public UsersController(App context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUsers(long id)
        {
            var users = await _context.Users.FindAsync(id);

            if (users == null)
            {
                return NotFound();
            }

            return users;
        }

        // GET: api/Users/email/admin%40admin.com
        [HttpGet("email/{email}")]
        public async Task<ActionResult> CheckIfUserExistsByEmail(string email)
        {
            var decodedEmail = HttpUtility.UrlDecode(email);
            var user = await _context.Users.Where(u => u.Email == decodedEmail).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return Ok();
        }


        // PUT: api/users/update
        [HttpPut("update/{email}")]
        public async Task<IActionResult> UpdateUserInfo(string email, UserDTO userDTO)
        {
            var decodedEmail = HttpUtility.UrlDecode(email);

            var user = await _context.Users.Where(u => u.Email == decodedEmail).SingleOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            if (userDTO.FirstName != null)
            {
                user.FirstName = userDTO.FirstName;
            }
            if (userDTO.LastName != null)
            {
                user.LastName = userDTO.LastName;
            }
            if (userDTO.Title != null)
            {
                user.Title = userDTO.Title;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(user.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(userDTO);
        }

        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Users>> PostUsers(Users users)
        {
            _context.Users.Add(users);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsers", new { id = users.Id }, users);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Users>> DeleteUsers(long id)
        {
            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }

            _context.Users.Remove(users);
            await _context.SaveChangesAsync();

            return users;
        }

        private bool UsersExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
