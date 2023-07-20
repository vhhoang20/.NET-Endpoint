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
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly APIDbContext _context;

        public OrderController(APIDbContext context)
        {
            _context = context;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
          if (_context.Orders == null)
          {
              return NotFound();
          }
            return await _context.Orders.ToListAsync();
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // PUT: api/Order/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutOrder(Order order)
        {
            var existingOder = await _context.Orders.FirstOrDefaultAsync(u => u.ID == order.ID);
            if (existingOder == null)
            {
                return NotFound("Order not found.");
            }

            // Update the properties of the existing user
            existingOder.date = order.date;
            existingOder.address = order.address;
            existingOder.price = order.price;
            existingOder.status = order.status;

            try
            {
                // Save the changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(existingOder.ID))
                {
                    return NotFound("Order not found.");
                }
                else
                {
                    throw;
                }
            }

            // Return the updated user
            return Ok("Modified success");
        }

        // POST: api/Order
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            var existingOrder = await _context.Orders.FirstOrDefaultAsync(u => u.ID == order.ID);
            if (existingOrder != null)
            {
                return Conflict("Username or email is already exist.");
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Registration successful
            return Ok(order);
        }

        // DELETE: api/Order/5
        [HttpDelete]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(u => u.ID == id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return Ok("Orders deleted successfully.");
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
