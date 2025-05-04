using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Presentation.ActionRequests.User;

namespace Presentation.Controllers;

public class RoleController : Controller
{
    private readonly RoleManager<Role> _roleManager;

    public RoleController(RoleManager<Role> roleManager)
    {
        _roleManager = roleManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return View(roles);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRoleActionRequest request)
    {
        if (ModelState.IsValid)
        {
            var result = await _roleManager.CreateAsync(new Role
            {
                Name = request.Name
            });
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }
        }
        return View(request);
    }
}