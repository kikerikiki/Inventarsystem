using Inventarsystem1.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Inventarsystem1.Controllers
{
    public class AccountController : Controller
    {
        private readonly LagerContext _context;
        private IConfiguration configuration;
        public AccountController(LagerContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;


        }


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [BindProperty]
        public LoginModel user { get; set; }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string returnUrl = null)
        {

            var userlist = _context.TblUser
             .Where(a => a.Email.Equals(user.Email));
            //.Where(a => a.Passwort.Equals(user.Passwort)).Include(a => a.TblUserRole).ThenInclude(a => a.Role).FirstOrDefault();


            var emailExist = _context.TblUser.Any(x => x.Email == user.Email);
            if (emailExist)
            {
                var model = _context.TblUser.Where(x => x.Email == user.Email).Include(x => x.TblUserRole).ThenInclude(a => a.Role).FirstOrDefault();


                //Kwennwort ist richtig aber altes Format
                if (userlist != null && model.Passwort.Equals(model.Passwort) && model.Passwort.Length < 20)
                {
                    //neues Format = Hash + Salt
                    // Generate a 128-bit salt using a sequence of
                    // cryptographically strong random bytes.
                    byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
                    string salt_ = Convert.ToBase64String(salt);
                    // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
                    string hashed = Convert.ToBase64String(
                        KeyDerivation.Pbkdf2(
                        password: user.Passwort!,
                        salt: salt,
                        prf: KeyDerivationPrf.HMACSHA256,
                        iterationCount: 100000,
                        numBytesRequested: 256 / 8));
                    //Daten ändern und in die DB speichern
                    model.Passwort = hashed;
                    model.Salt = salt_;
                    _context.TblUser.Update(model);
                    await _context.SaveChangesAsync();
                }
                var gw = configuration["Pepper"];
                //Wenn true dann Verschlüsselt ohne Pepper
                if (model.CheckHashCode(user.Passwort))
                {
                    // Generate a 128-bit salt using a sequence of
                    // cryptographically strong random bytes.
                    byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
                    string salt_ = Convert.ToBase64String(salt);
                    // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
                    string hashed = Convert.ToBase64String(
                        KeyDerivation.Pbkdf2(
                        password: user.Passwort + gw!,
                        salt: salt,
                        prf: KeyDerivationPrf.HMACSHA256,
                        iterationCount: 100000,
                        numBytesRequested: 256 / 8));
                    //Daten ändern und in die DB speichern
                    model.Passwort = hashed;
                    model.Salt = salt_;
                    _context.TblUser.Update(model);
                    await _context.SaveChangesAsync();
                }

                if (model.CheckHashCode(user.Passwort + gw))
                {


                    if (model != null)
                    {
                        var claims = new List<Claim>
                    {
                new Claim(ClaimTypes.Email, user.Email),     
                //new Claim(ClaimTypes.Role, "Admin")      
                
                    };
                        foreach (var item in model.TblUserRole)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, item.Role.RoleName));
                        }
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                        };
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                        if (returnUrl != null)
                            return Redirect(returnUrl);
                        else
                            return Redirect("https://localhost:7078/Lobby/Lobby");
                    }

                }
            }
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("https://localhost:7078/LogOut/Logout");
        }



    }
}
