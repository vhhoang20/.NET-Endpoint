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
    public class PaymentController : ControllerBase
    {
        private readonly APIDbContext _context;

        public PaymentController(APIDbContext context)
        {
            _context = context;
        }

        // GET: api/Payment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
          if (_context.Payments == null)
          {
              return NotFound();
          }
            return await _context.Payments.ToListAsync();
        }

        // GET: api/Payment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }

        // PUT: api/Payment/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutPayment(Payment payment)
        {
            // Find the existing by ID
            var existingPayment = await _context.Payments.FirstOrDefaultAsync(u => u.paymentId == payment.paymentId);
            if (existingPayment == null)
            {
                return NotFound("User not found.");
            }

            // Update the properties of the existing user
            existingPayment.CardNumber = payment.CardNumber;
            existingPayment.CardType = payment.CardType;
            existingPayment.balance = payment.balance;

            try
            {
                // Save the changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(payment.paymentId))
                {
                    return NotFound("Payment not found.");
                }
                else
                {
                    throw;
                }
            }

            // Return the updated user
            return Ok("Modified success");
        }

        // POST: api/Payment
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Payment>> PostPayment(Payment payment)
        {
            var existingPayment = await _context.Payments.FirstOrDefaultAsync(u => u.paymentId == payment.paymentId);
            if (existingPayment != null)
            {
                return Conflict("Payment is already exist.");
            }

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            // Registration successful
            return Ok(payment);
        }

        // DELETE: api/Payment/5
        [HttpDelete]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(u => u.paymentId == id);
            if (payment == null)
            {
                return NotFound();
            }

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            return Ok("Payment deleted successfully.");
        }

        private bool PaymentExists(int id)
        {
            return (_context.Payments?.Any(e => e.paymentId == id)).GetValueOrDefault();
        }
    }
}
