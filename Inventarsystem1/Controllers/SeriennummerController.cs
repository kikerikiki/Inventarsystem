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
    public class SeriennummerController : Controller
    {
        private readonly LagerContext _context;

        public SeriennummerController(LagerContext context)
        {
            _context = context;
        }

        // GET: Seriennummer
        public async Task<IActionResult> Index()
        {
            var lagerContext = _context.TblSeriennummer.Include(s => s.SeriennummerNavigation);
            return View(await lagerContext.ToListAsync());
        }

        // GET: Seriennummer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblSeriennummer == null)
            {
                return NotFound();
            }

            var seriennummer = await _context.TblSeriennummer
                .Include(s => s.SeriennummerNavigation)
                .FirstOrDefaultAsync(m => m.SeriennummerId == id);
            if (seriennummer == null)
            {
                return NotFound();
            }

            return View(seriennummer);
        }

        // GET: Seriennummer/Create
        public IActionResult Create()
        {
            ViewData["SeriennummerId"] = new SelectList(_context.TblProdukt, "ProduktId", "ProduktId");
            return View();
        }

        // POST: Seriennummer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SeriennummerId,Srnummer")] Seriennummer seriennummer)
        {
            ModelState.Remove("SeriennummerNavigation");
            if (ModelState.IsValid)
            {
                _context.Add(seriennummer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SeriennummerId"] = new SelectList(_context.TblProdukt, "ProduktId", "ProduktId", seriennummer.SeriennummerId);
            return View(seriennummer);
        }

        // GET: Seriennummer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblSeriennummer == null)
            {
                return NotFound();
            }

            var seriennummer = await _context.TblSeriennummer.FindAsync(id);
            if (seriennummer == null)
            {
                return NotFound();
            }
            ViewData["SeriennummerId"] = new SelectList(_context.TblProdukt, "ProduktId", "ProduktId", seriennummer.SeriennummerId);
            return View(seriennummer);
        }

        // POST: Seriennummer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SeriennummerId,Srnummer")] Seriennummer seriennummer)
        {
            if (id != seriennummer.SeriennummerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(seriennummer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeriennummerExists(seriennummer.SeriennummerId))
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
            ViewData["SeriennummerId"] = new SelectList(_context.TblProdukt, "ProduktId", "ProduktId", seriennummer.SeriennummerId);
            return View(seriennummer);
        }

        // GET: Seriennummer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblSeriennummer == null)
            {
                return NotFound();
            }

            var seriennummer = await _context.TblSeriennummer
                .Include(s => s.SeriennummerNavigation)
                .FirstOrDefaultAsync(m => m.SeriennummerId == id);
            if (seriennummer == null)
            {
                return NotFound();
            }

            return View(seriennummer);
        }

        // POST: Seriennummer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblSeriennummer == null)
            {
                return Problem("Entity set 'LagerContext.TblSeriennummer'  is null.");
            }
            var seriennummer = await _context.TblSeriennummer.FindAsync(id);
            if (seriennummer != null)
            {
                _context.TblSeriennummer.Remove(seriennummer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeriennummerExists(int id)
        {
          return _context.TblSeriennummer.Any(e => e.SeriennummerId == id);
        }
    }
}
