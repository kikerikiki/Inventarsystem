using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventarsystem1.Models;

namespace Inventarsystem1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIUserRoleController : ControllerBase
    {
        private readonly LagerContext _context;

        public APIUserRoleController(LagerContext context)
        {
            _context = context;
        }

        // GET: api/APIUserRole
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserRole>>> GetTblUserRole()
        {
            return await _context.TblUserRole.ToListAsync();
        }

        // GET: api/APIUserRole/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserRole>> GetUserRole(int id)
        {
            var userRole = await _context.TblUserRole.FindAsync(id);

            if (userRole == null)
            {
                return NotFound();
            }

            return userRole;
        }

        // PUT: api/APIUserRole/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserRole(int id, UserRole userRole)
        {
            if (id != userRole.UserRoleId)
            {
                return BadRequest();
            }

            _context.Entry(userRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserRoleExists(id))
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

        // POST: api/APIUserRole
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserRole>> PostUserRole(UserRole userRole)
        {
            _context.TblUserRole.Add(userRole);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserRole", new { id = userRole.UserRoleId }, userRole);
        }

        // DELETE: api/APIUserRole/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserRole(int id)
        {
            var userRole = await _context.TblUserRole.FindAsync(id);
            if (userRole == null)
            {
                return NotFound();
            }

            _context.TblUserRole.Remove(userRole);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserRoleExists(int id)
        {
            return _context.TblUserRole.Any(e => e.UserRoleId == id);
        }
    }
}
