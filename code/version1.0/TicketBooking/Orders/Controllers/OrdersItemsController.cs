using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orders.Models;

namespace Orders.Controllers
{
    [Route("api/OrdersItems")]
    [ApiController]
    public class OrdersItemsController : ControllerBase
    {
        private readonly OrdersContext _context;

        public OrdersItemsController(OrdersContext context)
        {
            _context = context;
        }

        // GET: api/OrdersItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrdersItem>>> GetOrdersItems()
        {
            return await _context.OrdersItems.ToListAsync();
        }

        // GET: api/OrdersItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrdersItem>> GetOrdersItem(long id)
        {
            var ordersItem = await _context.OrdersItems.FindAsync(id);

            if (ordersItem == null)
            {
                return NotFound();
            }

            return ordersItem;
        }

        // PUT: api/OrdersItems/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrdersItem(long id, OrdersItem ordersItem)
        {
            if (id != ordersItem.TicketId)
            {
                return BadRequest();
            }

            _context.Entry(ordersItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdersItemExists(id))
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

        // POST: api/OrdersItems
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<OrdersItem>> PostOrdersItem(OrdersItem ordersItem)
        {
            _context.OrdersItems.Add(ordersItem);
            await _context.SaveChangesAsync();

            // return CreatedAtAction("GetOrdersItem", new { id = ordersItem.ID }, ordersItem);
            return CreatedAtAction(nameof(GetOrdersItem), new { id = ordersItem.TicketId }, ordersItem);
        }

        // DELETE: api/OrdersItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<OrdersItem>> DeleteOrdersItem(long id)
        {
            var ordersItem = await _context.OrdersItems.FindAsync(id);
            if (ordersItem == null)
            {
                return NotFound();
            }

            _context.OrdersItems.Remove(ordersItem);
            await _context.SaveChangesAsync();

            return ordersItem;
        }

        private bool OrdersItemExists(long id)
        {
            return _context.OrdersItems.Any(e => e.TicketId == id);
        }

        // PlaceAnOrder: api/OrdersItems/PlaceAnOrder/5
        [HttpPut("PlaceAnOrder/{id}")]
        public async Task<IActionResult> PlaceAnOrder(long id, OrdersItem ordersItem)
        {
            if (id != ordersItem.TicketId)
            {
                return BadRequest();
            }
            
            var entity = _context.OrdersItems.Find(ordersItem.TicketId);
            _context.OrdersItems.Attach(entity);
            entity.UserId = ordersItem.UserId;
            entity.Status = 1;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdersItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Content("you have booked a ticket, please pay for it.");
        }

        // OrderSuccess: api/OrdersItems/OrderSuccess/5
        [HttpPut("OrderSuccess/{id}")]
        public async Task<IActionResult> OrderSuccess(long id, OrdersItem ordersItem)
        {
            if (id != ordersItem.TicketId)
            {
                return BadRequest();
            }

            var entity = _context.OrdersItems.Find(ordersItem.TicketId);
            _context.OrdersItems.Attach(entity);
            entity.Status = 2;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdersItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Content("order the ticket successfully");
        }

        // CancelOrder: api/OrdersItems/CancelOrder/5
        [HttpPut("CancelOrder/{id}")]
        public async Task<IActionResult> CancelOrder(long id, OrdersItem ordersItem)
        {
            if (id != ordersItem.TicketId)
            {
                return BadRequest();
            }

            var entity = _context.OrdersItems.Find(ordersItem.TicketId);
            _context.OrdersItems.Attach(entity);
            entity.UserId = 0;
            entity.Status = 0;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdersItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Content("cancel the ticket successfully");
        }

        // DeleteATicket: api/OrdersItems/DeleteATicket/5
        [HttpPut("DeleteATicket/{id}")]
        public async Task<IActionResult> DeleteATicket(long id, OrdersItem ordersItem)
        {
            if (id != ordersItem.TicketId)
            {
                return BadRequest();
            }

            var entity = _context.OrdersItems.Find(ordersItem.TicketId);
            _context.OrdersItems.Attach(entity);
            entity.Status = -1;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdersItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Content("delete the ticket successfully");
        }

        // GetAllTickets: api/OrdersItems/GetAllTickets
        [HttpGet("GetAllTickets")]
        public async Task<ActionResult<IEnumerable<OrdersItem>>> GetAllTickets()
        {
            var result = await _context.OrdersItems.ToListAsync();
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i].Status == -1)
                {
                    result.Remove(result[i]);
                }
            }
            return result;
        }

        // GetATicket: api/OrdersItems/GetATicket/5
        [HttpGet("GetATicket/{id}")]
        public async Task<ActionResult<OrdersItem>> GetATicket(long id)
        {
            var ordersItem = await _context.OrdersItems.FindAsync(id);

            if (ordersItem == null || ordersItem.Status == -1)
            {
                return NotFound();
            }

            return ordersItem;
        }
    }
}
