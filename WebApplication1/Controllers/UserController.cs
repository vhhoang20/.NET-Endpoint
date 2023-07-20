using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly APIDbContext _context;

        public UserController(APIDbContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            return await _context.Users.ToListAsync();
        }

        // GET: api/User
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutUser(User user)
        {
            // Find the existing user by ID
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.ID == user.ID);
            if (existingUser == null)
            {
                return NotFound("User not found.");
            }

            // Update the properties of the existing user
            existingUser.name = user.name;
            existingUser.mail = user.mail;
            existingUser.birth = user.birth;
            existingUser.sex = user.sex;
            existingUser.company = user.company;
            existingUser.home = user.home;
            existingUser.username = user.username;
            existingUser.password = user.password;

            try
            {
                // Save the changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.ID))
                {
                    return NotFound("User not found.");
                }
                else
                {
                    throw;
                }
            }

            // Return the updated user
            return Ok("Modified success");
        }



        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.username == user.username || u.mail == user.mail);
            if (existingUser != null)
            {
                return Conflict("Username or email is already exist.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Registration successful
            return Ok(user);
        }

        // DELETE: api/User
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("User deleted successfully.");
        }


        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
