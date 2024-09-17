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
    public class APIKategorieController : ControllerBase
    {
        private readonly LagerContext _context;

        public APIKategorieController(LagerContext context)
        {
            _context = context;
        }

        // GET: api/APIKategorie
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kategorie>>> GetTblKategorie()
        {
            return await _context.TblKategorie.ToListAsync();
        }

        // GET: api/APIKategorie/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Kategorie>> GetKategorie(int id)
        {
            var kategorie = await _context.TblKategorie.FindAsync(id);

            if (kategorie == null)
            {
                return NotFound();
            }

            return kategorie;
        }

        // PUT: api/APIKategorie/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKategorie(int id, [FromBody] Kategorie kategorie)
        {
            if (id != kategorie.KategorieId)
            {
                return BadRequest();
            }

            _context.Entry(kategorie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KategorieExists(id))
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

        // POST: api/APIKategorie
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Kategorie>> PostKategorie([FromBody] Kategorie kategorie )
        {
            _context.TblKategorie.Add(kategorie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKategorie", new { id = kategorie.KategorieId }, kategorie);
        }

        // DELETE: api/APIKategorie/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKategorie(int id)
        {
            var kategorie = await _context.TblKategorie.FindAsync(id);
            if (kategorie == null)
            {
                return NotFound();
            }

            _context.TblKategorie.Remove(kategorie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KategorieExists(int id)
        {
            return _context.TblKategorie.Any(e => e.KategorieId == id);
        }
    }
}
