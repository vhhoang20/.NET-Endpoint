using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthoController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult TestAutho()
        {
            return Ok("Authoried");
        }
    }
}
