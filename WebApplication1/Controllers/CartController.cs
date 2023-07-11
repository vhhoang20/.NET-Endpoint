using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        public static List<Order> orders = new List<Order>();
        public static List<OrderItem> orderItems = new List<OrderItem>();

        [HttpGet]
        public IActionResult GetAllOrders()
        {
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            var order = orders.Find(o => o.ID == id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPost]
        public IActionResult CreateOrder(Order newOrder)
        {
            orders.Add(newOrder);
            return CreatedAtAction(nameof(GetOrderById), new { id = newOrder.ID }, newOrder);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, Order updatedOrder)
        {
            var order = orders.Find(o => o.ID == id);
            if (order == null)
            {
                return NotFound();
            }

            order.status = updatedOrder.status;
            // Update other properties as needed

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var order = orders.Find(o => o.ID == id);
            if (order == null)
            {
                return NotFound();
            }

            orders.Remove(order);

            return NoContent();
        }

        [HttpGet("{id}/orderItems")]
        public IActionResult GetOrderItems(int id)
        {
            var items = orderItems.Where(oi => oi.orderID == id).ToList();
            if (items.Count == 0)
            {
                return NotFound("No order items found for the specified order.");
            }

            return Ok(items);
        }

        [HttpPost("{id}/orderItems")]
        public IActionResult AddOrderItems(int id, [FromBody] List<OrderItem> items)
        {
            var order = orders.Find(o => o.ID == id);
            if (order == null)
            {
                return NotFound();
            }

            foreach (var item in items)
            {
                item.orderID = id;
                orderItems.Add(item);
            }

            return Ok();
        }
    }
}
