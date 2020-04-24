using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Payment.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class paymentsController : ControllerBase
    {
        private readonly PaymentContext _context;

        public paymentsController(PaymentContext context)
        {
            _context = context;
        }

        // GET: api/payments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<payment>>> GetPayments()
        {
            return await _context.Payments.ToListAsync();
        }

        // GET: api/payments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<payment>> Getpayment(long id)
        {
            var payment = await _context.Payments.FindAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            return payment;
        }

        // PUT: api/payments/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> Putpayment(long id, payment payment)
        {
            if (id != payment.UserID)
            {
                return BadRequest();
            }

            _context.Entry(payment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!paymentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/payments
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<payment>> Postpayment(payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getpayment", new { id = payment.UserID }, payment);
        }

        // DELETE: api/payments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<payment>> Deletepayment(long id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            return payment;
        }

        private bool paymentExists(long id)
        {
            return _context.Payments.Any(e => e.UserID == id);
        }

        // PayAnOrder: api/payments/PayAnOrder/5
        [HttpPut("PayAnOrder/{id}")]
        public async Task<IActionResult> PayAnOrder(long id, payment payment)
        {
            if (id != payment.UserID)
            {
                return BadRequest();
            }

            // _context.Entry(payment).State = EntityState.Modified;
            var entity = _context.Payments.Find(payment.UserID);
            _context.Payments.Attach(entity);
            if(entity.Balance < payment.Balance)
            {
                return Content("sorry, you do not have enough money...");
            }
            entity.Balance -= payment.Balance;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!paymentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Content("pay for the ticket successfully");
        }

        // CancelAnOrder: api/payments/CancelAnOrder/5
        [HttpPut("CancelAnOrder/{id}")]
        public async Task<IActionResult> CancelAnOrder(long id, payment payment)
        {
            if (id != payment.UserID)
            {
                return BadRequest();
            }

            // _context.Entry(payment).State = EntityState.Modified;
            var entity = _context.Payments.Find(payment.UserID);
            _context.Payments.Attach(entity);
            entity.Balance += payment.Balance;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!paymentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
    }
}
