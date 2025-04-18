using System.Diagnostics;

using Business.DTOs.Products;

using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers;

public class AdminController : Controller
{


    public IActionResult Index()
    {

        List<GetAllProductsDto> products = new List<GetAllProductsDto>();
        return View(products);
    }




}
