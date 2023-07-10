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

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            return Ok(users);
        }

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

        [HttpPost]
        public IActionResult CreateUser(User newUser)
        {
            newUser.ID = users.Count + 1;
            users.Add(newUser);

            return CreatedAtAction(nameof(GetUserById), new { id = newUser.ID }, newUser);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User updatedUser)
        {
            var user = users.Find(u => u.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            user.name = updatedUser.name;
            user.mail = updatedUser.mail;
            user.birth = updatedUser.birth;
            user.sex = updatedUser.sex;
            user.company = updatedUser.company;
            user.home = updatedUser.home;
            user.username = updatedUser.username;
            user.password = updatedUser.password;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = users.Find(u => u.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            users.Remove(user);

            return NoContent();
        }
    }
}
