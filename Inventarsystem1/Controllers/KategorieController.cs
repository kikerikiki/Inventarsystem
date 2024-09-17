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
    public class KategorieController : Controller
    {
        private readonly LagerContext _context;

        public KategorieController(LagerContext context)
        {
            _context = context;
        }

        // GET: Kategorie
        public async Task<IActionResult> Index()
        {
              return View(await _context.TblKategorie.ToListAsync());
        }

        // GET: Kategorie/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblKategorie == null)
            {
                return NotFound();
            }

            var kategorie = await _context.TblKategorie
                .FirstOrDefaultAsync(m => m.KategorieId == id);
            if (kategorie == null)
            {
                return NotFound();
            }

            return View(kategorie);
        }

        // GET: Kategorie/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Kategorie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KategorieId,KategorieName")] Kategorie kategorie)
        {
            
            if (ModelState.IsValid)
            {
                //_context.Add(kategorie);
                //await _context.SaveChangesAsync();
                await _context.Database.ExecuteSqlInterpolatedAsync($"DECLARE @id int;exec spCreateKategorie @id output, {kategorie.KategorieName}");
                return RedirectToAction(nameof(Index));
            }
            return View(kategorie);
        }

        // GET: Kategorie/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblKategorie == null)
            {
                return NotFound();
            }

            var kategorie = await _context.TblKategorie.FindAsync(id);
            if (kategorie == null)
            {
                return NotFound();
            }
            return View(kategorie);
        }

        // POST: Kategorie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KategorieId,KategorieName")] Kategorie kategorie)
        {
            if (id != kategorie.KategorieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(kategorie);
                    //await _context.SaveChangesAsync();
                    await _context.Database.ExecuteSqlInterpolatedAsync($"exec spUpdateKategorie {kategorie.KategorieId},{kategorie.KategorieName}");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KategorieExists(kategorie.KategorieId))
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
            return View(kategorie);
        }

        // GET: Kategorie/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblKategorie == null)
            {
                return NotFound();
            }

            var kategorie = await _context.TblKategorie
                .FirstOrDefaultAsync(m => m.KategorieId == id);
            if (kategorie == null)
            {
                return NotFound();
            }

            return View(kategorie);
        }

        // POST: Kategorie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblKategorie == null)
            {
                return Problem("Entity set 'LagerContext.TblKategorie'  is null.");
            }
            var kategorie = await _context.TblKategorie.FindAsync(id);
            if (kategorie != null)
            {
                //_context.TblKategorie.Remove(kategorie);
                await _context.Database.ExecuteSqlInterpolatedAsync($"exec spDeleteKategorie {kategorie.KategorieId}");
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KategorieExists(int id)
        {
          return _context.TblKategorie.Any(e => e.KategorieId == id);
        }
    }
}
