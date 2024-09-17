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
    public class APIRoleController : ControllerBase
    {
        private readonly LagerContext _context;

        public APIRoleController(LagerContext context)
        {
            _context = context;
        }

        // GET: api/APIRole
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetTblRole()
        {
            return await _context.TblRole.ToListAsync();
        }

        // GET: api/APIRole/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            var role = await _context.TblRole.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return role;
        }

        // PUT: api/APIRole/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int id, [FromBody] Role role)
        {
            if (id != role.RoleId)
            {
                return BadRequest();
            }

            _context.Entry(role).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
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

        // POST: api/APIRole
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Role>> PostRole([FromBody] Role role)
        {
            _context.TblRole.Add(role);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRole", new { id = role.RoleId }, role);
        }

        // DELETE: api/APIRole/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _context.TblRole.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            _context.TblRole.Remove(role);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoleExists(int id)
        {
            return _context.TblRole.Any(e => e.RoleId == id);
        }
    }
}
