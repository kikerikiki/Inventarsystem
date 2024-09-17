using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventarsystem1.Models;
using Microsoft.AspNetCore.Authorization;

namespace Inventarsystem1.Controllers
{
    [Authorize]
    public class ProduktController : Controller
    {
        private readonly LagerContext _context;

        public ProduktController(LagerContext context)
        {
            _context = context;
        }

        // GET: Produkt
        public async Task<IActionResult> Index()
        {
            var lagerContext = _context.TblProdukt.Include(p => p.Artikel).Include(p => p.Raum).Include(p => p.User);
            return View(await lagerContext.ToListAsync());
        }

        // GET: Produkt/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblProdukt == null)
            {
                return NotFound();
            }

            var produkt = await _context.TblProdukt
                .Include(p => p.Artikel)
                .Include(p => p.Raum)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ProduktId == id);
            if (produkt == null)
            {
                return NotFound();
            }

            return View(produkt);
        }

        // GET: Produkt/Create
        public IActionResult Create()
        {
            ViewData["ArtikelId"] = new SelectList(_context.TblArtikel, "ArtikelId", "ArtikelId");
            ViewData["RaumId"] = new SelectList(_context.TblRaum, "RaumId", "RaumId");
            ViewData["UserId"] = new SelectList(_context.TblUser, "UserId", "UserId");
            return View();
        }

        // POST: Produkt/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProduktId,UserId,ArtikelId,Produktname,Kommentar,RaumId,Datum")] Produkt produkt)
        {
            ModelState.Remove("Artikel");
            ModelState.Remove("User");
            ModelState.Remove("Raum");
            if (ModelState.IsValid)
            {
                //_context.Add(produkt);
                //await _context.SaveChangesAsync();
                await _context.Database.ExecuteSqlInterpolatedAsync($"DECLARE @id int;exec spCreateProdukt @id output,{produkt.UserId}, {produkt.ArtikelId}, {produkt.Produktname}, {produkt.Kommentar}, {produkt.RaumId}, {produkt.Datum}");
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtikelId"] = new SelectList(_context.TblArtikel, "ArtikelId", "ArtikelId", produkt.ArtikelId);
            ViewData["RaumId"] = new SelectList(_context.TblRaum, "RaumId", "RaumId", produkt.RaumId);
            ViewData["UserId"] = new SelectList(_context.TblUser, "UserId", "UserId", produkt.UserId);
            return View(produkt);
        }

        // GET: Produkt/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblProdukt == null)
            {
                return NotFound();
            }

            var produkt = await _context.TblProdukt.FindAsync(id);
            if (produkt == null)
            {
                return NotFound();
            }
            ViewData["ArtikelId"] = new SelectList(_context.TblArtikel, "ArtikelId", "ArtikelId", produkt.ArtikelId);
            ViewData["RaumId"] = new SelectList(_context.TblRaum, "RaumId", "RaumId", produkt.RaumId);
            ViewData["UserId"] = new SelectList(_context.TblUser, "UserId", "UserId", produkt.UserId);
            return View(produkt);
        }

        // POST: Produkt/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProduktId,UserId,ArtikelId,Produktname,Kommentar,RaumId,Datum")] Produkt produkt)
        {
            if (id != produkt.ProduktId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(produkt);
                    //await _context.SaveChangesAsync();
                    await _context.Database.ExecuteSqlInterpolatedAsync($"exec spUpdateProdukt {produkt.ProduktId}, {produkt.UserId}, {produkt.ArtikelId}, {produkt.Produktname}, {produkt.Kommentar}, {produkt.RaumId}, {produkt.Datum}");

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProduktExists(produkt.ProduktId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtikelId"] = new SelectList(_context.TblArtikel, "ArtikelId", "ArtikelId", produkt.ArtikelId);
            ViewData["RaumId"] = new SelectList(_context.TblRaum, "RaumId", "RaumId", produkt.RaumId);
            ViewData["UserId"] = new SelectList(_context.TblUser, "UserId", "UserId", produkt.UserId);
            return View(produkt);
        }

        // GET: Produkt/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblProdukt == null)
            {
                return NotFound();
            }

            var produkt = await _context.TblProdukt
                .Include(p => p.Artikel)
                .Include(p => p.Raum)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ProduktId == id);
            if (produkt == null)
            {
                return NotFound();
            }

            return View(produkt);
        }

        // POST: Produkt/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblProdukt == null)
            {
                return Problem("Entity set 'LagerContext.TblProdukt'  is null.");
            }
            var produkt = await _context.TblProdukt.FindAsync(id);
            if (produkt != null)
            {
                //_context.TblProdukt.Remove(produkt);
                await _context.Database.ExecuteSqlInterpolatedAsync($"exec spDeleteProdukt {produkt.ProduktId}");
            }
            
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProduktExists(int id)
        {
          return _context.TblProdukt.Any(e => e.ProduktId == id);
        }
    }
}
