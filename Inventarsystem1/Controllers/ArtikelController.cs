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
    public class ArtikelController : Controller
    {
        private readonly LagerContext _context;

        public ArtikelController(LagerContext context)
        {
            _context = context;
        }

        // GET: Artikel
        public async Task<IActionResult> Index()
        {
            var lagerContext = _context.TblArtikel.Include(a => a.Kategorie);
            return View(await lagerContext.ToListAsync());
        }

        // GET: Artikel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblArtikel == null)
            {
                return NotFound();
            }

            var artikel = await _context.TblArtikel
                .Include(a => a.Kategorie)
                .FirstOrDefaultAsync(m => m.ArtikelId == id);
            if (artikel == null)
            {
                return NotFound();
            }

            return View(artikel);
        }

        // GET: Artikel/Create
        public IActionResult Create()
        {
            ViewData["KategorieId"] = new SelectList(_context.TblKategorie, "KategorieId", "KategorieId");
            return View();
        }

        // POST: Artikel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArtikelId,ArtikelName,KategorieId,HatSeriennummer")] Artikel artikel)
        {
            ModelState.Remove("Kategorie");
            if (ModelState.IsValid)
            {
                //_context.Add(artikel);
                //await _context.SaveChangesAsync();
                await _context.Database.ExecuteSqlInterpolatedAsync($"DECLARE @id int;exec spCreateArtikel @id output, {artikel.ArtikelName}, {artikel.KategorieId}, {artikel.HatSeriennummer}");
                return RedirectToAction(nameof(Index));
            }
            ViewData["KategorieId"] = new SelectList(_context.TblKategorie, "KategorieId", "KategorieId", artikel.KategorieId);
            return View(artikel);
        }

        // GET: Artikel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblArtikel == null)
            {
                return NotFound();
            }

            var artikel = await _context.TblArtikel.FindAsync(id);
            if (artikel == null)
            {
                return NotFound();
            }
            ViewData["KategorieId"] = new SelectList(_context.TblKategorie, "KategorieId", "KategorieId", artikel.KategorieId);
            return View(artikel);
        }

        // POST: Artikel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArtikelId,ArtikelName,KategorieId,HatSeriennummer")] Artikel artikel)
        {
            if (id != artikel.ArtikelId)
            {
                return NotFound();
            }
            
            ModelState.Remove("Kategorie");
            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(artikel);
                    //await _context.SaveChangesAsync();
                    await _context.Database.ExecuteSqlInterpolatedAsync($"exec spUpdateArtikel {artikel.ArtikelId}, {artikel.ArtikelName}, {artikel.KategorieId}, {artikel.HatSeriennummer}");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtikelExists(artikel.ArtikelId))
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
            ViewData["KategorieId"] = new SelectList(_context.TblKategorie, "KategorieId", "KategorieId", artikel.KategorieId);
            return View(artikel);
        }

        // GET: Artikel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblArtikel == null)
            {
                return NotFound();
            }

            var artikel = await _context.TblArtikel
                .Include(a => a.Kategorie)
                .FirstOrDefaultAsync(m => m.ArtikelId == id);
            if (artikel == null)
            {
                return NotFound();
            }

            return View(artikel);
        }

        // POST: Artikel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblArtikel == null)
            {
                return Problem("Entity set 'LagerContext.TblArtikel'  is null.");
            }
            var artikel = await _context.TblArtikel.FindAsync(id);
            if (artikel != null)
            {
                //_context.TblArtikel.Remove(artikel);
                await _context.Database.ExecuteSqlInterpolatedAsync($"exec spDeleteArtikel {artikel.ArtikelId}");
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtikelExists(int id)
        {
          return _context.TblArtikel.Any(e => e.ArtikelId == id);
        }
    }
}
