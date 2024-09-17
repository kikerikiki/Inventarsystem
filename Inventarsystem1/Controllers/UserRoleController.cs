using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventarsystem1.Models;
using System.Security.Claims;

namespace Inventarsystem1.Controllers
{
    public class UserRoleController : Controller
    {
        private readonly LagerContext _context;

        public UserRoleController(LagerContext context)
        {
            _context = context;
        }

        // GET: UserRole
        public async Task<IActionResult> Index()
        {
            List<Claim> roleClaims = HttpContext.User.FindAll(ClaimTypes.Role).ToList();
            var roles = new List<string>();

            foreach (var role in roleClaims)
            {
                roles.Add(role.Value);
            }
            if(roles.Contains("Admin"))
            {
                var inventarContext = _context.TblUserRole.Include(u => u.Role).Include(u => u.User);
                return View(await inventarContext.ToListAsync());
            }
            else
            {
                return RedirectToAction("NotAuthorized", "User");
            }
            
        }

        // GET: UserRole/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblUserRole == null)
            {
                return NotFound();
            }

            var userRole = await _context.TblUserRole
                .Include(u => u.Role)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserRoleId == id);
            if (userRole == null)
            {
                return NotFound();
            }

            return View(userRole);
        }

        // GET: UserRole/Create

        public IActionResult Create()
        { 
            ViewData["RoleId"] = new SelectList(_context.TblRole, "RoleId", "RoleId");
            ViewData["UserId"] = new SelectList(_context.TblUser, "UserId", "UserId");
            return View();
        }

        // POST: UserRole/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserRoleId,RoleId,UserId")] UserRole userRole)
        {
            ModelState.Remove("Role");
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                //_context.Add(userRole);
                //await _context.SaveChangesAsync();
                await _context.Database.ExecuteSqlInterpolatedAsync($"DECLARE @id int;exec spCreateUserRole @id output,{userRole.RoleId}, {userRole.UserId}");

                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.TblRole, "RoleId", "RoleId", userRole.RoleId);
            ViewData["UserId"] = new SelectList(_context.TblUser, "UserId", "UserId", userRole.UserId);
            return View(userRole);
        }

        // GET: UserRole/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblUserRole == null)
            {
                return NotFound();
            }

            var userRole = await _context.TblUserRole.FindAsync(id);
            if (userRole == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.TblRole, "RoleId", "RoleId", userRole.RoleId);
            ViewData["UserId"] = new SelectList(_context.TblUser, "UserId", "UserId", userRole.UserId);
            return View(userRole);
        }

        // POST: UserRole/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserRoleId,RoleId,UserId")] UserRole userRole)
        {
            if (id != userRole.UserRoleId)
            {
                return NotFound();
            }
            ModelState.Remove("Salt");
            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(userRole);
                    //await _context.SaveChangesAsync();
                    await _context.Database.ExecuteSqlInterpolatedAsync($"exec spUpdateUserRole {userRole.UserRoleId}, {userRole.RoleId}, {userRole.UserId}");

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserRoleExists(userRole.UserRoleId))
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
            ViewData["RoleId"] = new SelectList(_context.TblRole, "RoleId", "RoleId", userRole.RoleId);
            ViewData["UserId"] = new SelectList(_context.TblUser, "UserId", "UserId", userRole.UserId);
            return View(userRole);
        }

        // GET: UserRole/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblUserRole == null)
            {
                return NotFound();
            }

            var userRole = await _context.TblUserRole
                .Include(u => u.Role)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserRoleId == id);
            if (userRole == null)
            {
                return NotFound();
            }

            return View(userRole);
        }

        // POST: UserRole/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblUserRole == null)
            {
                return Problem("Entity set 'LagerContext.TblUserRole'  is null.");
            }
            var userRole = await _context.TblUserRole.FindAsync(id);
            if (userRole != null)
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($"exec spDeleteUserRole {userRole.UserRoleId}");
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserRoleExists(int id)
        {
          return _context.TblUserRole.Any(e => e.UserRoleId == id);
        }
        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}
