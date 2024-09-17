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
    public class RaumController : Controller
    {
        private readonly LagerContext _context;

        public RaumController(LagerContext context)
        {
            _context = context;
        }

        // GET: Raum
        public async Task<IActionResult> Index()
        {
            var lagerContext = _context.TblRaum.Include(r => r.User);
            return View(await lagerContext.ToListAsync());
        }

        // GET: Raum/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblRaum == null)
            {
                return NotFound();
            }

            var raum = await _context.TblRaum
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.RaumId == id);
            if (raum == null)
            {
                return NotFound();
            }

            return View(raum);
        }

        // GET: Raum/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.TblUser, "UserId", "UserId");
            return View();
        }

        // POST: Raum/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RaumId,RaumName,Stockwerk,UserId")] Raum raum)
        {
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                //_context.Add(raum);
                //await _context.SaveChangesAsync();
                await _context.Database.ExecuteSqlInterpolatedAsync($"DECLARE @id int;exec spCreateRaum @id output,{raum.UserId}, {raum.RaumName}, {raum.Stockwerk}");
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.TblUser, "UserId", "UserId", raum.UserId);
            return View(raum);
        }

        // GET: Raum/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblRaum == null)
            {
                return NotFound();
            }

            var raum = await _context.TblRaum.FindAsync(id);
            if (raum == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.TblUser, "UserId", "UserId", raum.UserId);
            return View(raum);
        }

        // POST: Raum/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RaumId,RaumName,Stockwerk,UserId")] Raum raum)
        {
            if (id != raum.RaumId)
            {
                return NotFound();
            }

            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(raum);
                    //await _context.SaveChangesAsync();
                    await _context.Database.ExecuteSqlInterpolatedAsync($"exec spUpdateRaum {raum.RaumId}, {raum.UserId}, {raum.RaumName}, {raum.Stockwerk}");
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RaumExists(raum.RaumId))
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
            ViewData["UserId"] = new SelectList(_context.TblUser, "UserId", "UserId", raum.UserId);
            return View(raum);
        }

        // GET: Raum/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblRaum == null)
            {
                return NotFound();
            }

            var raum = await _context.TblRaum
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.RaumId == id);
            if (raum == null)
            {
                return NotFound();
            }

            return View(raum);
        }

        // POST: Raum/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblRaum == null)
            {
                return Problem("Entity set 'LagerContext.TblRaum'  is null.");
            }
            var raum = await _context.TblRaum.FindAsync(id);
            if (raum != null)
            {
                //_context.TblRaum.Remove(raum);
                await _context.Database.ExecuteSqlInterpolatedAsync($"exec spDeleteRaum {raum.RaumId}");
                return RedirectToAction(nameof(Index));
            }
            
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RaumExists(int id)
        {
          return _context.TblRaum.Any(e => e.RaumId == id);
        }
    }
}
