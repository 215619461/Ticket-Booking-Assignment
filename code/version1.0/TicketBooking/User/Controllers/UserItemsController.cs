using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using User.Models;

namespace User.Controllers
{
    [Route("api/UserItems")]
    [ApiController]
    public class UserItemsController : ControllerBase
    {
        private readonly UserContext _context;

        public UserItemsController(UserContext context)
        {
            _context = context;
        }

        // GET: api/UserItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserItem>>> GetUserItems()
        {
            return await _context.UserItems.ToListAsync();
        }

        // GET: api/UserItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserItem>> GetUserItem(long id)
        {
            var userItem = await _context.UserItems.FindAsync(id);

            if (userItem == null)
            {
                return NotFound();
            }

            return userItem;
        }

        // PUT: api/UserItems/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserItem(long id, UserItem userItem)
        {
            if (id != userItem.UserId)
            {
                return BadRequest();
            }

            _context.Entry(userItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserItemExists(id))
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

        // POST: api/UserItems
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<UserItem>> PostUserItem(UserItem userItem)
        {
            _context.UserItems.Add(userItem);
            await _context.SaveChangesAsync();

            // return CreatedAtAction("GetUserItem", new { id = userItem.Id }, userItem);.
            return CreatedAtAction(nameof(GetUserItem), new { id = userItem.UserId }, userItem);
        }

        // DELETE: api/UserItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserItem>> DeleteUserItem(long id)
        {
            var userItem = await _context.UserItems.FindAsync(id);
            if (userItem == null)
            {
                return NotFound();
            }

            _context.UserItems.Remove(userItem);
            await _context.SaveChangesAsync();

            return userItem;
        }

        private bool UserItemExists(long id)
        {
            return _context.UserItems.Any(e => e.UserId == id);
        }

        // DeleteAUser: api/UserItems/DeleteAUser/5
        [HttpPut("DeleteAUser/{id}")]
        public async Task<IActionResult> DeleteAUser(long id, UserItem userItem)
        {
            if (id != userItem.UserId)
            {
                return BadRequest();
            }

            // _context.Entry(userItem).State = EntityState.Modified;
            var entity = _context.UserItems.Find(userItem.UserId);
            _context.UserItems.Attach(entity);
            entity.Status = -1;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Content("success delete the user");
        }

        // GetAllUser: api/UserItems/GetAllUser
        [HttpGet("GetAllUser")]
        public async Task<ActionResult<IEnumerable<UserItem>>> GetAllUser()
        {
            var result = await _context.UserItems.ToListAsync();
            for (int i = 0; i < result.Count; i++)
            {
                if(result[i].Status == -1)
                {
                    result.Remove(result[i]);
                }
            }
            return result;
        }

        // GetAUser: api/UserItems/GetAUser/5
        [HttpGet("GetAUser/{id}")]
        public async Task<ActionResult<UserItem>> GetAUser(long id)
        {
            var userItem = await _context.UserItems.FindAsync(id);

            if (userItem == null || userItem.Status == -1)
            {
                return NotFound();
            }

            return userItem;
        }
    }
}
