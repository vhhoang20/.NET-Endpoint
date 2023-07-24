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
    public class ShoppingItemController : ControllerBase
    {
        private readonly APIDbContext _context;

        public ShoppingItemController(APIDbContext context)
        {
            _context = context;
        }

        // GET: api/OrderItem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingItem>>> GetOrderItems()
        {
          if (_context.ShoppingItems == null)
          {
              return NotFound();
          }
            return await _context.ShoppingItems.ToListAsync();
        }

        // GET: api/OrderItem/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingItem>> GetOrderItem(int id)
        {
            var orderItem = await _context.ShoppingItems.FirstOrDefaultAsync(u => u.Id == id);

            if (orderItem == null)
            {
                return NotFound();
            }

            return Ok(orderItem);
        }

        // PUT: api/OrderItem/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutOrderItem(ShoppingItem orderItem)
        {
            var existingOrderItem = await _context.ShoppingItems.FirstOrDefaultAsync(u => u.Id == orderItem.Id);
            if (existingOrderItem == null)
            {
                return NotFound("Order item not found.");
            }

            // Update the properties of the existing
            existingOrderItem.product = orderItem.product;
            existingOrderItem.quantity = orderItem.quantity;
            existingOrderItem.status = orderItem.status;
            existingOrderItem.cartId = orderItem.cartId;
            existingOrderItem.orderId = orderItem.orderId;

            try
            {
                // Save the changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderItemExists(orderItem.Id))
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
        public async Task<ActionResult<ShoppingItem>> PostOrderItem(ShoppingItem orderItem)
        {
            var existingOrderItem = await _context.ShoppingItems.FirstOrDefaultAsync(u => u.cartId == orderItem.cartId);
            if (existingOrderItem != null)
            {
                return Conflict("Order item is already exist.");
            }

            _context.ShoppingItems.Add(orderItem);
            await _context.SaveChangesAsync();

            // Registration successful
            return Ok(orderItem);
        }

        // DELETE: api/OrderItem
        [HttpDelete]
        public async Task<IActionResult> DeleteOrderItem(int? cartId)
        {
            var orderItem = await _context.ShoppingItems.FirstOrDefaultAsync(u => u.cartId == cartId);
            if (orderItem == null)
            {
                return NotFound();
            }

            _context.ShoppingItems.Remove(orderItem);
            await _context.SaveChangesAsync();

            return Ok("Deleted successfully.");
        }

        private bool OrderItemExists(int shoppingItemID)
        {
            return (_context.ShoppingItems?.Any(e => e.Id == shoppingItemID)).GetValueOrDefault();
        }
    }
}
