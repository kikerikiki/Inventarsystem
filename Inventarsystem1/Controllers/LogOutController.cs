using Microsoft.AspNetCore.Mvc;

namespace Inventarsystem1.Controllers
{
    public class LogOutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return View();
        }
    }
}
