using Microsoft.AspNetCore.Mvc;

namespace Inventarsystem1.Controllers
{
    public class LobbyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Lobby()
        {
            return View();
        }
    }
}
