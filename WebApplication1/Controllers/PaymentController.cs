using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        public static List<Payment> payments = new List<Payment>();

        [HttpGet]
        public IActionResult GetAllPayments()
        {
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public IActionResult GetPaymentById(int id)
        {
            var payment = payments.Find(p => p.Id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }

        [HttpPost]
        public IActionResult CreatePayment(Payment newPayment)
        {
            newPayment.Id = payments.Count + 1;
            payments.Add(newPayment);

            return CreatedAtAction(nameof(GetPaymentById), new { id = newPayment.Id }, newPayment);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePayment(int id, Payment updatedPayment)
        {
            var payment = payments.Find(p => p.Id == id);
            if (payment == null)
            {
                return NotFound();
            }

            payment.CardType = updatedPayment.CardType;
            payment.CardNumber = updatedPayment.CardNumber;
            payment.balance = updatedPayment.balance;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePayment(int id)
        {
            var payment = payments.Find(p => p.Id == id);
            if (payment == null)
            {
                return NotFound();
            }

            payments.Remove(payment);

            return NoContent();
        }
    }
}
