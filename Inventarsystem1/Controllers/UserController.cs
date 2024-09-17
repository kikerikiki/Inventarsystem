using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventarsystem1.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Inventarsystem1.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly LagerContext _context;

        public UserController(LagerContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            List<Claim> roleClaims = HttpContext.User.FindAll(ClaimTypes.Role).ToList();
            var roles = new List<string>();

            foreach (var role in roleClaims)
            {
                roles.Add(role.Value);
            }
            if (roles.Contains("Admin"))
            {
                //var inventarContext = _context.TblUserRole.Include(u => u.Role).Include(u => u.User);
                return View(await _context.TblUser.ToListAsync());
            }
            else
            {
                return RedirectToAction("NotAuthorized", "User");
            }
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblUser == null)
            {
                return NotFound();
            }

            var user = await _context.TblUser
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Email,Nachname,Passwort")] User user)
        {
            ModelState.Remove("Salt");
            if (ModelState.IsValid)
            {
                //_context.Add(user);
                //await _context.SaveChangesAsync();
                await _context.Database.ExecuteSqlInterpolatedAsync($"DECLARE @id int;exec spCreateUser @id output,{user.Email}, {user.Nachname}, {user.Passwort}");
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblUser == null)
            {
                return NotFound();
            }

            var user = await _context.TblUser.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Email,Nachname,Passwort")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }
            ModelState.Remove("Salt");
            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(user);
                    //await _context.SaveChangesAsync();
                    await _context.Database.ExecuteSqlInterpolatedAsync($"exec spUpdateUser {user.UserId}, {user.Email}, {user.Nachname}, {user.Passwort}");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
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
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblUser == null)
            {
                return NotFound();
            }

            var user = await _context.TblUser
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblUser == null)
            {
                return Problem("Entity set 'LagerContext.TblUser'  is null.");
            }
            var user = await _context.TblUser.FindAsync(id);
            if (user != null)
            {
                //_context.TblUser.Remove(user);
                await _context.Database.ExecuteSqlInterpolatedAsync($"exec spDeleteUser {user.UserId}");
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.TblUser.Any(e => e.UserId == id);
        }

        public ActionResult ShowLogin()
        {

            return View();
        }
        public ActionResult Login()
        {

            //string connectionString = @"Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog = Inventarsystem; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
            //SqlConnection conn = new SqlConnection(connectionString);
            //conn.Open();


            //SqlCommand checkEmail = new SqlCommand("SELECT UserId, Vorname, Nachname, Passwort FROM [TblUser]WHERE ([Vorname] = @Vorname),([Nachname] = @Nachname) AND ([Passwort] = @Passwort)", conn);
            //checkEmail.Parameters.AddWithValue("Vorname", Vorname);
            //checkEmail.Parameters.AddWithValue("Nachname", Nachname);
            //checkEmail.Parameters.AddWithValue("Password", Passwort);
            //checkEmail.CommandType = System.Data.CommandType.Text;

            //SqlDataReader dr = checkEmail.ExecuteReader();
            //DataTable dt = new DataTable();
            //dt.Load(dr);
            //conn.Close();



            //if (dt.Rows.Count > 0)
            //{
            //    string IDEnd = dt.Rows[0]["UserId"].ToString();
            //    string VornameEnd = dt.Rows[0]["Vorname"].ToString();
            //    string NachnameEnd = dt.Rows[0]["Nachname"].ToString();
            //    string PasswortEnd = dt.Rows[0]["Passwort"].ToString();
            //    HttpContext.Session.SetString("UserId", dt.Rows[0]["UserId"]?.ToString()?.Trim() ?? "");
            //    HttpContext.Session.SetString("Nachname", dt.Rows[0]["Nachname"]?.ToString()?.Trim() ?? "");


            //}
            //else
            //{
            //    Console.WriteLine("Passwort oder Email sind ungültig");
            //}


            return View();
        }
        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}
