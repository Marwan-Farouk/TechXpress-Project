using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Business.DTOs.Products;
using Business.Managers.Products;

namespace Presentation.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductManager _productManager;

    public HomeController(ILogger<HomeController> logger, IProductManager productManager)
    {
        _logger = logger;
        _productManager = productManager;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productManager.GetAllProductsAsync();
        return View(products);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
