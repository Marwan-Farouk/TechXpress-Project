using Microsoft.AspNetCore.Mvc;
using YourProject.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace YourProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Login(string username, string password, bool keep)
        {
            if (string.IsNullOrEmpty(username))
            {
                ViewBag.UserError = "Please insert username";
                return View();
            }

            if (string.IsNullOrEmpty(password))
            {
                ViewBag.PassError = "Please insert password";
                return View();
            }

            string md5Pass = GetMd5Hash(password);

            var user = _context.Users
                               .Where(u => u.Username == username && u.Md5Pass == md5Pass)
                               .FirstOrDefault();

            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetInt32("UserId", user.Id);

                if (keep)
                {
                    Response.Cookies.Append("Username", user.Username, new Microsoft.AspNetCore.Http.CookieOptions { Expires = DateTime.Now.AddHours(1) });
                    Response.Cookies.Append("Password", md5Pass, new Microsoft.AspNetCore.Http.CookieOptions { Expires = DateTime.Now.AddHours(1) });
                }

                return RedirectToAction("Home");
            }
            else
            {
                ViewBag.UserError = "Wrong username or password";
                return View();
            }
        }

        private string GetMd5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        public IActionResult Home()
        {
            return View();
        }
    }
}