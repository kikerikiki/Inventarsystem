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
    public class APIProduktController : ControllerBase
    {
        private readonly LagerContext _context;

        public APIProduktController(LagerContext context)
        {
            _context = context;
        }

        // GET: api/APIProdukt
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produkt>>> GetTblProdukt()
        {
            return await _context.TblProdukt.ToListAsync();
        }

        // GET: api/APIProdukt/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Produkt>> GetProdukt(int id)
        {
            var produkt = await _context.TblProdukt.FindAsync(id);

            if (produkt == null)
            {
                return NotFound();
            }

            return produkt;
        }

        // PUT: api/APIProdukt/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProdukt(int id, [FromBody] Produkt produkt)
        {
            if (id != produkt.ProduktId)
            {
                return BadRequest();
            }

            _context.Entry(produkt).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProduktExists(id))
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

        // POST: api/APIProdukt
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Produkt>> PostProdukt([FromBody] Produkt produkt)
        {
            _context.TblProdukt.Add(produkt);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProdukt", new { id = produkt.ProduktId }, produkt);
        }

        // DELETE: api/APIProdukt/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProdukt(int id)
        {
            var produkt = await _context.TblProdukt.FindAsync(id);
            if (produkt == null)
            {
                return NotFound();
            }

            _context.TblProdukt.Remove(produkt);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProduktExists(int id)
        {
            return _context.TblProdukt.Any(e => e.ProduktId == id);
        }
    }
}
