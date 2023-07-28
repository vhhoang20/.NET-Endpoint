using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthoController : ControllerBase
    {
        [HttpGet("secured")]
        [Authorize(Policy = "CustomPolicy")]
        public IActionResult GetUserData()
        {
            return Ok("This is secured data.");
        }
    }
}
