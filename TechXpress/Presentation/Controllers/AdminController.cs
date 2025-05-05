using System.Diagnostics;
using System.Threading.Tasks;
using Business.DTOs.Products;
using Business.Managers.Roles;
using Business.Managers.Users;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization; /////////////////
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Presentation.ActionRequests.Admin;
using Presentation.Models;
using Presentation.ViewModel.Admin;

namespace Presentation.Controllers;


[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IUserManager _userManager;
    private readonly IRoleManager _roleManager;

    public AdminController(IUserManager userManager,IRoleManager roleManager)
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
        var users = _userManager.GetUsers();
        var roles = _roleManager.GetRoles();

        var model = new UserRoleViewModel
        {
            Users = users.Select(user => new SelectListItem { Text = user.UserName, Value = user.Id.ToString() }).ToList(),
            Roles = roles.Select(role => new SelectListItem { Text = role.Name, Value = role.Id.ToString() }).ToList(),
        };
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> AssignUserRole(UserRoleViewModel request)
    {
        if (ModelState.IsValid) {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());
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
        var users = _userManager.GetUsers();
        return View(users);
    }

    [HttpGet]
    public async Task<IActionResult> ListUserRoles()
    {
        // Use ToListAsync() for async database operations
        var users = _userManager.GetUsers();
        var userRolesList = new List<UserRolesViewModel>();

        foreach (var user in users)
        {
            var userRolesVM = new UserRolesViewModel
            {
                UserId = user.Id, // Uncommented this as it's needed for the form
                UserName = user.UserName,
                Roles = (await _userManager.GetRolesAsync(user)).ToList()
            };
            userRolesList.Add(userRolesVM);
        }

        // Use async version and cache the roles
        ViewBag.AllRoles = _roleManager.GetRoles()
            .Select(role => role.Name);

        return View(userRolesList);
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // Add this for security
    public async Task<IActionResult> EditUserRoles(string userId, List<string> roles)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest("User ID is required");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        try
        {
            var currentRoles = await _userManager.GetRolesAsync(user);
            var rolesToAdd = roles.Except(currentRoles).ToList();
            var rolesToRemove = currentRoles.Except(roles).ToList();

            if (rolesToRemove.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            }

            if (rolesToAdd.Any())
            {
                await _userManager.AddToRolesAsync(user, rolesToAdd);
            }

            TempData["SuccessMessage"] = "Roles updated successfully";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error updating roles: {ex.Message}";
        }

        return RedirectToAction(nameof(ListUserRoles));
    }
    [HttpGet]
    public async Task<IActionResult> EditUser(int id)
    {
        // جلب بيانات المستخدم
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return NotFound();
        }

        var model = new EditUserViewModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            BirthDate = user.BirthDate
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(UpdateUserActionRequest request)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            // تحديث بيانات المستخدم
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.BirthDate = request.BirthDate;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "User data updated successfully!";
                return RedirectToAction(nameof(ListUsers));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
        }

        return View(request);
    }


}
