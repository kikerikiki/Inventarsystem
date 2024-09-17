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
    public class APIArtikelController : ControllerBase
    {
        private readonly LagerContext _context;

        public APIArtikelController(LagerContext context)
        {
            _context = context;
        }

        // GET: api/APIArtikel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Artikel>>> GetTblArtikel()
        {
            return await _context.TblArtikel.ToListAsync();
        }

        // GET: api/APIArtikel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Artikel>> GetArtikel(int id )
        {
            var artikel = await _context.TblArtikel.FindAsync(id);

            if (artikel == null)
            {
                return NotFound();
            }

            return artikel;
        }

        // PUT: api/APIArtikel/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtikel(int id, [FromBody] Artikel artikel)
        {
            if (id != artikel.ArtikelId)
            {
                return BadRequest();
            }

            _context.Entry(artikel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtikelExists(id))
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

        // POST: api/APIArtikel
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Artikel>> PostArtikel([FromBody] Artikel artikel )
        {
            _context.TblArtikel.Add(artikel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArtikel", new { id = artikel.ArtikelId }, artikel);
        }

        // DELETE: api/APIArtikel/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtikel(int id)
        {
            var artikel = await _context.TblArtikel.FindAsync(id);
            if (artikel == null)
            {
                return NotFound();
            }

            _context.TblArtikel.Remove(artikel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArtikelExists(int id)
        {
            return _context.TblArtikel.Any(e => e.ArtikelId == id);
        }
    }
}
