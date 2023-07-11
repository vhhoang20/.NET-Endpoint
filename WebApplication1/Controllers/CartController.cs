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

        [HttpPost("{id}/addProduct")]
        public IActionResult AddProductToCart(int id, [FromBody] OrderItem orderItem)
        {
            var cart = carts.Find(c => c.ID == id);
            if (cart == null)
            {
                return NotFound();
            }

            orderItem.cartID = id;
            orderItem.status = "InCart";
            orderItems.Add(orderItem);

            return Ok();
        }

        [HttpGet("{id}/getProducts")]
        public IActionResult GetCartProducts(int id)
        {
            var productsInCart = orderItems
                .Where(oi => oi.cartID == id && oi.status == "InCart")
                .Select(oi => new
                {
                    oi.productID,
                    oi.quantity
                })
                .ToList();

            return Ok(productsInCart);
        }

        [HttpDelete("{id}/removeProduct/{productID}")]
        public IActionResult RemoveProductFromCart(int id, int productID)
        {
            var orderItem = orderItems.Find(oi => oi.cartID == id && oi.productID == productID && oi.status == "InCart");
            if (orderItem == null)
            {
                return NotFound();
            }

            orderItems.Remove(orderItem);

            return NoContent();
        }

        [HttpPost("{id}/placeOrder")]
        public IActionResult PlaceOrder(int id)
        {
            var cart = carts.Find(c => c.ID == id);
            if (cart == null)
            {
                return NotFound();
            }

            var cartItems = orderItems.Where(oi => oi.cartID == id && oi.status == "InCart").ToList();
            if (cartItems.Count == 0)
            {
                return BadRequest("No products in the cart.");
            }

            foreach (var item in cartItems)
            {
                item.status = "Ordered";
            }

            return Ok("Order placed successfully.");
        }
    }
}
