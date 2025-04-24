using Microsoft.AspNetCore.Mvc;
using RegisterProject.Models;

namespace RegisterProject.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            bool hasError = false;

            if (string.IsNullOrWhiteSpace(model.Username))
            {
                model.UsernameError = "Please enter username";
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(model.Email))
            {
                model.EmailError = "Please enter email";
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(model.Password))
            {
                model.PasswordError = "Please enter password";
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(model.Gender) || 
                !(model.Gender == "male" || model.Gender == "female"))
            {
                model.GenderError = "Please select a valid gender";
                hasError = true;
            }

            if (hasError)
            {
                return View(model);
            }


            return RedirectToAction("Index", "Home");
        }
    }
}