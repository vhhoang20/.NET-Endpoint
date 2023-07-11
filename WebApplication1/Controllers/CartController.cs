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
        public static List<CartProduct> cartProducts = new List<CartProduct>();

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

        [HttpGet("{cartId}/cartproducts")]
        public IActionResult GetCartProducts(int cartId)
        {
            var products = cartProducts.FindAll(cp => cp.cartID == cartId);
            return Ok(products);
        }

        [HttpPost("{cartId}/cartproducts")]
        public IActionResult AddCartProduct(int cartId, CartProduct cartProduct)
        {
            cartProduct.cartID = cartId;
            cartProducts.Add(cartProduct);
            return CreatedAtAction(nameof(GetCartProductById), new { cartId = cartProduct.cartID, id = cartProduct.cartID }, cartProduct);
        }

        [HttpGet("{cartId}/cartproducts/{id}")]
        public IActionResult GetCartProductById(int cartId, int id)
        {
            var cartProduct = cartProducts.Find(cp => cp.cartID == cartId && cp.cartID == id);
            if (cartProduct == null)
            {
                return NotFound();
            }

            return Ok(cartProduct);
        }

        [HttpPut("{cartId}/cartproducts/{id}")]
        public IActionResult UpdateCartProduct(int cartId, int id, CartProduct updatedCartProduct)
        {
            var cartProduct = cartProducts.Find(cp => cp.cartID == cartId && cp.cartID == id);
            if (cartProduct == null)
            {
                return NotFound();
            }

            cartProduct.cartID = updatedCartProduct.cartID;
            cartProduct.quantity = updatedCartProduct.quantity;

            return NoContent();
        }

        [HttpDelete("{cartId}/cartproducts/{id}")]
        public IActionResult DeleteCartProduct(int cartId, int id)
        {
            var cartProduct = cartProducts.Find(cp => cp.cartID == cartId && cp.cartID == id);
            if (cartProduct == null)
            {
                return NotFound();
            }

            cartProducts.Remove(cartProduct);

            return NoContent();
        }
    }
}
