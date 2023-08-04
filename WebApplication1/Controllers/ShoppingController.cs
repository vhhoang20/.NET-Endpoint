using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {
        private readonly APIDbContext _context;

        public ShoppingController(APIDbContext context)
        {
            _context = context;
        }

        [HttpGet("addProduct")]
        [Authorize("Customer")]
        public IActionResult AddToCart(Product product, int quantity)
        {
            var userId = User.FindFirstValue("sub");

            var existingCart = _context.Carts.FirstOrDefault(cart => cart.customerID == userId);

            var existingItem = _context.ShoppingItems.FirstOrDefault(item => item.CartId == existingCart.ID && item.ProductId == product.productId);
            
            if (existingItem == null)
            {
                var newItem = new ShoppingItem
                {
                    CartId = existingCart.ID,
                    ProductId = product.productId,
                    quantity = quantity,
                    status = "Cart"
                };

                // Add the new item to the context and save changes
                _context.ShoppingItems.Add(newItem);
                _context.SaveChanges();
            }
            else
            {
                return BadRequest("Already in cart");
            }

            return Ok("Add successful");
        }

        [HttpGet("placeOrder")]
        [Authorize("Customer")]
        public IActionResult PlaceOrder()
        {
            var userId = User.FindFirstValue("sub");

            var userInfo = _context.Users.FirstOrDefault(u => u.Id == userId);

            var existingCart = _context.Carts.FirstOrDefault(cart => cart.customerID == userId);

            var existingCartItem = _context.ShoppingItems.Where(item => item.CartId == existingCart.ID).ToList();

            if (existingCartItem == null)
            {
                return BadRequest("No item in cart");
            }
            else
            {
                var totalPrice = 0;

                foreach (var item in existingCartItem)
                {
                    var product = _context.Products.FirstOrDefault(p => p.productId == item.ProductId);
                    totalPrice += product.price * item.quantity;
                }

                var newOrder = new Order
                {
                    customerID = userId,
                    date = DateTime.Now,
                    address = userInfo.home,
                    price = totalPrice,
                    status = "Sellers are preparing your order"
                };

                // Save the order to the database
                _context.Orders.Add(newOrder);
                _context.SaveChanges();

                foreach (var item in existingCartItem)
                {

                    item.OrderId = newOrder.orderId;
                    item.CartId = null;
                    item.status = "Order";
                }

                _context.SaveChanges();
            }


            return Ok("Order placed successfully.");
        }


    }
}
