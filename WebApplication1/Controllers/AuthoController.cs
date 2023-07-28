using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthoController : ControllerBase
    {
        [HttpGet]
        [Authorize (Policy = "CustomPolicy")]
        public IActionResult TestAutho()
        {
            return Ok("Authoried");
        }

        [HttpGet("temp")]
        public IActionResult temp()
        {
            return Ok("UnAuthoried");
        }
    }
}
