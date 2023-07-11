using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        public static List<Cart> carts = new List<Cart>();
        public static List<OrderItem> orderItems = new List<OrderItem>();

        [HttpGet("{id}")]
        public IActionResult GetCartById(int id)
        {
            var cart = carts.Find(c => c.ID == id);
            if (cart == null)
            {
                return NotFound();
            }

            return Ok(cart);
        }

        [HttpPost]
        public IActionResult CreateCart(Cart cart)
        {
            carts.Add(cart);
            return CreatedAtAction(nameof(GetCartById), new { id = cart.ID }, cart);
        }

        [HttpGet("{cartId}/orderitems")]
        public IActionResult GetOrderItems(int cartId)
        {
            var items = orderItems.FindAll(oi => oi.cartID == cartId);
            return Ok(items);
        }

        [HttpPost("{cartId}/orderitems")]
        public IActionResult AddOrderItem(int cartId, OrderItem orderItem)
        {
            orderItem.cartID = cartId;
            orderItems.Add(orderItem);
            return CreatedAtAction(nameof(GetOrderItemById), new { cartId = orderItem.cartID, id = orderItem.orderID }, orderItem);
        }

        [HttpGet("{cartId}/orderitems/{id}")]
        public IActionResult GetOrderItemById(int cartId, int id)
        {
            var orderItem = orderItems.Find(oi => oi.cartID == cartId && oi.orderID == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return Ok(orderItem);
        }

        [HttpPut("{cartId}/orderitems/{id}")]
        public IActionResult UpdateOrderItem(int cartId, int id, OrderItem updatedOrderItem)
        {
            var orderItem = orderItems.Find(oi => oi.cartID == cartId && oi.orderID == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            orderItem.productID = updatedOrderItem.productID;
            orderItem.quantity = updatedOrderItem.quantity;
            orderItem.status = updatedOrderItem.status;

            return NoContent();
        }

        [HttpDelete("{cartId}/orderitems/{id}")]
        public IActionResult DeleteOrderItem(int cartId, int id)
        {
            var orderItem = orderItems.Find(oi => oi.cartID == cartId && oi.orderID == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            orderItems.Remove(orderItem);

            return NoContent();
        }
    }
}
