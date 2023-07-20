using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly APIDbContext _context;

        public AuthenticationController(APIDbContext context)
        {
            _context = context;
        }

        // POST: api/Login
        [HttpPost("Login")]
        public async Task<ActionResult> Login(Login account)
        {
            var foundUser = await _context.Users.FirstOrDefaultAsync(u => u.username == account.UserName && u.password == account.Password);

            if (foundUser != null)
            {
                return Ok(foundUser);
            }

            return NotFound();
        }

        // POST: /authentication/register
        [HttpPost("Register")]
        public async Task<ActionResult> Register(Register account)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.username == account.UserName || u.mail == account.mail);
            if (existingUser != null)
            {
                return BadRequest("Username or email is already taken.");
            }

            // Save the new user to the database
            var newUser = new User
            {
                username = account.UserName,
                password = account.Password,
                mail = account.mail
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // Registration successful
            return Ok("Registration successful.");
        }
    }
}
