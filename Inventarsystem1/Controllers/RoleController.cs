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
    public class RoleController : Controller
    {
        private readonly LagerContext _context;

        public RoleController(LagerContext context)
        {
            _context = context;
        }

        // GET: Role
        public async Task<IActionResult> Index()
        {
              return View(await _context.TblRole.ToListAsync());
        }

        // GET: Role/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblRole == null)
            {
                return NotFound();
            }

            var role = await _context.TblRole
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // GET: Role/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Role/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoleId,RoleName")] Role role)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(role);
                //await _context.SaveChangesAsync();
                await _context.Database.ExecuteSqlInterpolatedAsync($"DECLARE @id int;exec spCreateRole @id output,{role.RoleName}");
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Role/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblRole == null)
            {
                return NotFound();
            }

            var role = await _context.TblRole.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // POST: Role/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoleId,RoleName")] Role role)
        {
            if (id != role.RoleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(role);
                    await _context.Database.ExecuteSqlInterpolatedAsync($"exec spUpdateRole{role.RoleId}, {role.RoleName}");

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(role.RoleId))
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
            return View(role);
        }

        // GET: Role/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblRole == null)
            {
                return NotFound();
            }

            var role = await _context.TblRole
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // POST: Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblRole == null)
            {
                return Problem("Entity set 'LagerContext.TblRole'  is null.");
            }
            var role = await _context.TblRole.FindAsync(id);
            if (role != null)
            {
                _context.TblRole.Remove(role);
            }

            //await _context.SaveChangesAsync();
            await _context.Database.ExecuteSqlInterpolatedAsync($"exec spDeleteRole {role.RoleId}");
            return RedirectToAction(nameof(Index));
        }

        private bool RoleExists(int id)
        {
          return _context.TblRole.Any(e => e.RoleId == id);
        }
    }
}
