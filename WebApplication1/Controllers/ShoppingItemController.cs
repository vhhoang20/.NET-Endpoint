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
    public class ProductController : ControllerBase
    {
        private readonly APIDbContext _context;

        public ProductController(APIDbContext context)
        {
            _context = context;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            return await _context.Products.ToListAsync();
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Product/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutProduct(Product product)
        {
            // Find the existing user by ID
            var existingProduct = await _context.Products.FirstOrDefaultAsync(u => u.productId == product.productId);
            if (existingProduct == null)
            {
                return NotFound("Product not found.");
            }

            // Update the properties of the existing user
            existingProduct.name = product.name;
            existingProduct.description = product.description;
            existingProduct.price = product.price;
            existingProduct.rate = product.rate;

            try
            {
                // Save the changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.productId))
                {
                    return NotFound("Product not found.");
                }
                else
                {
                    throw;
                }
            }

            // Return the updated user
            return Ok("Modified success");
        }

        // POST: api/Product
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Registration successful
            return Ok(product);
        }

        // DELETE: api/Product/5
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.productId == id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok("Product deleted successfully.");
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.productId == id)).GetValueOrDefault();
        }
    }
}