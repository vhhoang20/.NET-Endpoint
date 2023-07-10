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

        [HttpPost]
        public IActionResult CreateProduct(Product newProduct)
        {
            newProduct.ID = products.Count + 1;
            products.Add(newProduct);

            return CreatedAtAction(nameof(GetProductById), new { id = newProduct.ID }, newProduct);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, Product updatedProduct)
        {
            var product = products.Find(p => p.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            product.name = updatedProduct.name;
            product.description = updatedProduct.description;
            product.price = updatedProduct.price;
            product.rate = updatedProduct.rate;

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
    }
}
