using System.Diagnostics;

using Business.DTOs.Products;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Models;

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

    public IActionResult Index()
    {

        List<GetAllProductsDto> products = new List<GetAllProductsDto>();
        return View(products);
    }

    [HttpGet]
    public async Task<IActionResult> AssignUserRole()
    {
        var users = await _userManager.Users.ToListAsync();
        var roles = await _roleManager.Roles.ToListAsync();
        return View();
    }
    // [HttpPost]
    // public IActionResult AssignUserRole()
    // {
    //     return View();
    // }


}
