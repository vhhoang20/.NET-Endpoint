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
    public class OrderItemController : ControllerBase
    {
        private readonly APIDbContext _context;

        public OrderItemController(APIDbContext context)
        {
            _context = context;
        }

        // GET: api/OrderItem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems()
        {
          if (_context.OrderItems == null)
          {
              return NotFound();
          }
            return await _context.OrderItems.ToListAsync();
        }

        // GET: api/OrderItem/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItem>> GetOrderItem(int id)
        {
            var orderItem = await _context.OrderItems.FirstOrDefaultAsync(u => u.productID == id);

            if (orderItem == null)
            {
                return NotFound();
            }

            return Ok(orderItem);
        }

        // PUT: api/OrderItem/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutOrderItem(OrderItem orderItem)
        {
            var existingOrderItem = await _context.OrderItems.FirstOrDefaultAsync(u => u.cartID == orderItem.cartID || u.orderID == orderItem.orderID);
            if (existingOrderItem == null)
            {
                return NotFound("Order item not found.");
            }

            // Update the properties of the existing
            existingOrderItem.productID = orderItem.productID;
            existingOrderItem.quantity = orderItem.quantity;
            existingOrderItem.status = orderItem.status;

            try
            {
                // Save the changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderItemExists(orderItem.orderID, orderItem.cartID))
                {
                    return NotFound("Order item not found.");
                }
                else
                {
                    throw;
                }
            }

            // Return the updated user
            return Ok("Modified success");
        }

        // POST: api/OrderItem
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderItem>> PostOrderItem(OrderItem orderItem)
        {
            var existingOrderItem = await _context.OrderItems.FirstOrDefaultAsync(u => u.orderID == orderItem.orderID || u.cartID == orderItem.cartID);
            if (existingOrderItem != null)
            {
                return Conflict("Order item is already exist.");
            }

            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();

            // Registration successful
            return Ok(orderItem);
        }

        // DELETE: api/OrderItem/5
        [HttpDelete]
        public async Task<IActionResult> DeleteOrderItem(int? orderId, int? cartId)
        {
            var orderItem = await _context.OrderItems.FirstOrDefaultAsync(u => u.orderID == orderId || u.cartID == cartId);
            if (orderItem == null)
            {
                return NotFound();
            }

            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();

            return Ok("Deleted successfully.");
        }

        private bool OrderItemExists(int orderId, int cartId)
        {
            return (_context.OrderItems?.Any(e => e.orderID == orderId && e.cartID == cartId)).GetValueOrDefault();
        }
    }
}
