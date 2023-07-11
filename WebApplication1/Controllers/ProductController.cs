using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public static List<Product> products = new List<Product>();

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = products.Find(p => p.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, string name, string description, int price, float rate)
        {
            var product = products.Find(p => p.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            product.name = name;
            product.description = description;
            product.price = price;
            product.rate = rate;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = products.Find(p => p.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            products.Remove(product);

            return NoContent();
        }

        [HttpPost("{id}/addToCart")]
        public IActionResult AddToCart(int id, int numberOfProduct)
        {
            var product = products.Find(p => p.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            
            return Ok();
        }

        [HttpGet("query")]
        public IActionResult QueryProducts(string name, string startPrice, string endPrice, float? startRate, float? endRate)
        {
            int? parsedStartPrice = null;
            int? parsedEndPrice = null;

            if (!string.IsNullOrEmpty(startPrice) && int.TryParse(startPrice, out int parsedStart))
            {
                parsedStartPrice = parsedStart;
            }

            if (!string.IsNullOrEmpty(endPrice) && int.TryParse(endPrice, out int parsedEnd))
            {
                parsedEndPrice = parsedEnd;
            }

            var queriedProducts = products.Where(p =>
                (string.IsNullOrEmpty(name) || p.name.Contains(name)) &&
                (!parsedStartPrice.HasValue || p.price >= parsedStartPrice.Value) &&
                (!parsedEndPrice.HasValue || p.price <= parsedEndPrice.Value) &&
                (!startRate.HasValue || p.rate >= startRate.Value) &&
                (!endRate.HasValue || p.rate <= endRate.Value)
            ).ToList();

            return Ok(queriedProducts);
        }

    }
}
