using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public static List<User> users = new List<User>();

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = users.Find(u => u.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost("login")]
        public IActionResult Login(string username, string password)
        {
            var user = users.Find(u => u.username == username && u.password == password);
            if (user == null)
            {
                return Unauthorized();
            }

            return Ok();
        }

        [HttpPost("forgetpassword")]
        public IActionResult ForgetPassword(string mail)
        {
            var user = users.Find(u => u.mail == mail);
            if (user == null)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPut("updatepassword")]
        public IActionResult UpdatePassword(string username, string oldPassword, string newPassword)
        {
            var user = users.Find(u => u.username == username && u.password == oldPassword);
            if (user == null)
            {
                return Unauthorized();
            }

            // Update the password for the user
            user.password = newPassword;

            return NoContent();
        }

        [HttpPut("updateprofile")]
        public IActionResult UpdateProfile(int id, string name, string mail, DateOnly birth, bool sex, string company, string home)
        {
            var user = users.Find(u => u.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            // Update the profile information for the user
            user.name = name;
            user.mail = mail;
            user.birth = birth;
            user.sex = sex;
            user.company = company;
            user.home = home;

            return NoContent();
        }
    }
}
