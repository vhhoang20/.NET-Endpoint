using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using System;
using System.Text.RegularExpressions;

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

        [HttpGet("{id}/balance")]
        public IActionResult GetPaymentBalance(int id)
        {
            var payment = payments.Find(p => p.Id == id);
            if (payment == null)
            {
                return NotFound();
            }

            int balance = payment.balance;
            return Ok(balance);
        }

        [HttpPut("{id}/recalculate")]
        public IActionResult RecalculatePayment(int id, int deduction)
        {
            var payment = payments.Find(p => p.Id == id);
            if (payment == null)
            {
                return NotFound();
            }

            payment.balance = payment.balance - deduction;
            return NoContent();
        }
    }
}
