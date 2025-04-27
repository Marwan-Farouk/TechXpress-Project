using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModel;
using DataAccess.Contexts;
using System.Linq;

namespace Presentation.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

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
            var user = _context.Users.FirstOrDefault();

            if (user == null)
                return RedirectToAction("Login", "Account");

            var model = new UserProfileViewModel
            {
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                PhoneNumber = user.userPhones?.FirstOrDefault()?.PhoneNumber ?? "Not Provided"
            };

            return View(model);
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
