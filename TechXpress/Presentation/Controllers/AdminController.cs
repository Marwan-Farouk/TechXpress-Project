using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers;

public class AdminController : Controller
{


    public IActionResult Index()
    {
        return View();
    }


}
