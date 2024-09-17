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
    public class APIRaumController : ControllerBase
    {
        private readonly LagerContext _context;

        public APIRaumController(LagerContext context)
        {
            _context = context;
        }

        // GET: api/APIRaum
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Raum>>> GetTblRaum()
        {
            return await _context.TblRaum.ToListAsync();
        }

        // GET: api/APIRaum/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Raum>> GetRaum(int id)
        {
            var raum = await _context.TblRaum.FindAsync(id);

            if (raum == null)
            {
                return NotFound();
            }

            return raum;
        }

        // PUT: api/APIRaum/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRaum(int id,[FromBody] Raum raum)
        {
            if (id != raum.RaumId)
            {
                return BadRequest();
            }

            _context.Entry(raum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RaumExists(id))
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

        // POST: api/APIRaum
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Raum>> PostRaum([FromBody] Raum raum)
        {
            _context.TblRaum.Add(raum);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRaum", new { id = raum.RaumId }, raum);
        }

        // DELETE: api/APIRaum/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRaum(int id)
        {
            var raum = await _context.TblRaum.FindAsync(id);
            if (raum == null)
            {
                return NotFound();
            }

            _context.TblRaum.Remove(raum);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RaumExists(int id)
        {
            return _context.TblRaum.Any(e => e.RaumId == id);
        }
    }
}
