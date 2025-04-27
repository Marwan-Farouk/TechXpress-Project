using Business.DTOs.Users;
using Business.Managers.Users;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionRequests;

namespace Presentation.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAddressManager _addressManager;

        public UserController(UserManager<User> userManager,SignInManager<User> signInManager,IAddressManager addressManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: true);

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
    }
}