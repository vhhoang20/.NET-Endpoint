﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [AllowAnonymous]
    [Route("api")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly string _jwtSecret;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly APIDbContext _context;

        public AuthenticationController(APIDbContext context,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            string jwtSecret,
            ILogger<AuthenticationController> logger)

        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtSecret = jwtSecret;
            _logger = logger;
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
                    if (!result.Succeeded)
                    {
                        // Password is incorrect, return Unauthorized
                        return Unauthorized();
                    }

                    var claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Name, user.UserName)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: "http://localhost:5157",
                        audience: "myApi",
                        claims: claims,
                        expires: DateTime.UtcNow.AddHours(1),
                        signingCredentials: creds);

                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
                }
            }

            return Unauthorized();

        }

        // POST: /auth/logout
        [HttpPost("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    await _signInManager.SignOutAsync(); // Sign out the user
                    _logger.LogInformation($"User {user.UserName} logged out.");
                    return Ok(new { message = "Logged out successfully." });
                }
                else
                {
                    return BadRequest("User not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout.");
                return StatusCode(500, new { message = "An error occurred during logout." });
            }
        }

        // POST: /auth/register
        [HttpPost("Register")]
        public async Task<IActionResult> Register(Register model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Mail,
                    PasswordHash = model.Password
                };

                var exists = await _userManager.FindByNameAsync(model.UserName);
                if (exists != null)
                {
                    ModelState.AddModelError("UserName", "Username alread exists");
                    return BadRequest("Username alread exists");
                }

                exists = await _userManager.FindByEmailAsync(model.Mail);
                if (exists != null)
                {
                    ModelState.AddModelError("Email", "Email already exists");
                    return BadRequest("Email alread exists");

                }

                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddClaimAsync(user, new Claim("Role", "customer"));

                if (result.Succeeded)
                {
                    var cart = new Cart
                    {
                        customerID = user.Id
                        // Add other cart properties as needed
                    };

                    // Save the cart to the database
                    _context.Carts.Add(cart);
                    _context.SaveChanges();

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Name, user.UserName)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: "http://localhost:5157",
                        audience: "myApi",
                        claims: claims,
                        expires: DateTime.UtcNow.AddHours(1),
                        signingCredentials: creds
                    );

                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
                }
            }
            return Unauthorized();
        }
    }
}
