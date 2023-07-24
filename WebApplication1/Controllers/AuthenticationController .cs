using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AuthenticationController(
            SignInManager<User> signInManager,
            UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // POST: /auth/login
        [HttpPost("Login")]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        // Redirect to the home page or some other page after successful login
                        return Ok("Login successful.");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            // If login fails, return the login view with validation errors
            return BadRequest("Invalid login credentials.");
        }

        // POST: /auth/logout
        [HttpPost("Logout")]
        [Authorize] // Require the user to be authenticated for this action
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Logout successful.");
        }

        // POST: /auth/register
        [HttpPost("Register")]
        public async Task<IActionResult> Register(Register model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByNameAsync(model.UserName);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Username already exists.");
                    return BadRequest("Username already exists.");
                }

                var newUser = new User
                {
                    UserName = model.UserName,
                    Email = model.Mail
                };

                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    // Redirect to the login page or some other page after successful registration
                    return Ok("Registration successful.");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            // If registration fails, return the registration view with validation errors
            return BadRequest("Registration failed.");
        }

        // GET: /auth/isauthenticated
        [HttpGet("IsAuthenticated")]
        public IActionResult IsAuthenticated()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return Ok(true);
            }

            return Ok(false);
        }
    }
}
