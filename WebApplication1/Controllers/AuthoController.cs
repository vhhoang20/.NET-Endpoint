using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AuthoController : ControllerBase
    {
        private readonly ILogger<AuthoController> _logger;

        public AuthoController(ILogger<AuthoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            if (User.Identity.IsAuthenticated)
            {
                _logger.LogInformation("User is authenticated.");
                return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
            }
            else
            {
                _logger.LogInformation("User is not authenticated.");
                return new JsonResult("User is not authenticated.");
            }
        }

        [HttpGet("custom")]
        [Authorize("Customer")]
        public IActionResult Got()
        {
            return Ok("customer");
        }

        [HttpGet("admin")]
        [Authorize("Admin")]
        public IActionResult gut()
        {
            return Ok("admin");
        }
    }
}
