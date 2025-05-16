using Business.DTOs.Users;
using Business.Managers.Users;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionRequests.User;

namespace Presentation.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly IAddressManager _addressManager;

        public UserController(IUserManager userManager,IAddressManager addressManager)
        {
            _userManager = userManager;
            _addressManager = addressManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterActionRequest request, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    BirthDate = request.BirthDate,
                    Email = request.Email,
                    UserName = $"{request.FirstName}_{request.LastName}",
                    DateCreated = DateTime.UtcNow,
                    PhoneNumber = request.PhoneNumber
                };

                var result = await _userManager.CreateUserAsync(user, request.Password);

                if (result.Succeeded)
                {
                    foreach(var address in request.Addresses)
                    {
                        await _addressManager.AddAddress(user.Id, new AddressDto
                        {
                            Country = address.Country,
                            City = address.City,
                            Street = address.Street,
                            BuildingNumber = address.BuildingNumber,
                            ApartmentNumber = address.ApartmentNumber
                        });
                    }

                    await _userManager.AddToRoleAsync(user, request.Role);
                    await _userManager.SignInAsync(user, rememberMe: true);

                    return returnUrl == null ? RedirectToAction("Index", "Home") : Redirect(returnUrl);
                } else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("Invalid Credentials", error.Description);
                    }
                }
            }
            return View(request);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginActionRequest request, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user != null)
                {
                    var passValid = await _userManager.CheckPasswordAsync(user, request.Password);
                    if (passValid)
                    {
                        await _userManager.SignInAsync(user, rememberMe: request.RememberMe);
                        return returnUrl == null ? RedirectToAction("Index", "Home") : Redirect(returnUrl);
                    }
                }
            }
            ModelState.AddModelError("Invalid Credentials", "Invalid UserName Or Password !");
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                await _userManager.SignOutAsync();
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
