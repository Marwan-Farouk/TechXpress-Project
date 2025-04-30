using System.Diagnostics;
using System.Threading.Tasks;
using Business.DTOs.Products;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Presentation.Models;
using Presentation.ViewModel.Admin;

namespace Presentation.Controllers;

public class AdminController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public AdminController(UserManager<User> userManager,RoleManager<Role> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }
    [HttpGet]
    public IActionResult Index()
    {

        List<GetAllProductsDto> products = new List<GetAllProductsDto>();
        return View(products);
    }

    [HttpGet]
    public async Task<IActionResult> AssignUserRole()
    {
        var users = _userManager.Users.ToList();
        var roles = _roleManager.Roles.ToList();

        var model = new UserRoleViewModel
        {
            Users = users.Select(user => new SelectListItem { Text = user.UserName, Value = user.Id.ToString() }).ToList(),
            Roles = roles.Select(role => new SelectListItem { Text = role.Name, Value = role.Id.ToString() }).ToList(),
        };
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> AssignUserRole(UserAddressViewModel request)
    {
        if (ModelState.IsValid) {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            var role = await _roleManager.FindByIdAsync(request.Id.ToString());
            if (user == null || role == null)
            {
                ModelState.AddModelError("Invalid data", "Invalid role or user");
                return View(request);
            }
            var result = await _userManager.AddToRoleAsync(user, role.Name);
            if (result.Succeeded) {
                TempData["SuccessMessage"] = "User has been successfully Assigned to the role";
                return RedirectToAction(nameof(AssignUserRole));
            }
            foreach (var error in result.Errors) {

                ModelState.AddModelError(error.Code, error.Description);
            }

        }
        
        return View(request);
    }
    [HttpGet]
    public async Task<IActionResult> ListUsers()
    {
        var users = _userManager.Users.ToList();
        return View(users);
    }

    [HttpGet]
    public async Task<IActionResult> ListUserRoles()
    {
        var users = _userManager.Users.ToList();

        var userRolesList = new List<UserRolesViewModel>();

        foreach (var user in users)
        {
            var userRolesVM = new UserRolesViewModel
            {
                //UserId = user.Id,
                UserName = user.UserName,
                Roles = await _userManager.GetRolesAsync(user)
            };
            userRolesList.Add(userRolesVM);
        }

        ViewBag.AllRoles = _roleManager.Roles
            .Select(role => role.Name)
            .ToList();

        return View(userRolesList);

    }
    [HttpPost]
    public async Task<IActionResult> EditUserRoles(string userId, List<string> roles)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        var rolesToAdd = roles.Except(currentRoles).ToList();
        var rolesToRemove = currentRoles.Except(roles).ToList();

        await _userManager.AddToRolesAsync(user, rolesToAdd);

        await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

        return RedirectToAction(nameof(ListUserRoles));

    }
}
