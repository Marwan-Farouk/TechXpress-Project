using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Orders()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Addresses()
        {
            return View();
        }

        public IActionResult Wishlist()
        {
            return View();
        }
    }
}