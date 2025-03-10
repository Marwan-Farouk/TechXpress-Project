using Microsoft.AspNetCore.Mvc;
using MyProject.Business.Services;

namespace MyProject.Presentation.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ProjectService _projectService = new();
        
        public IActionResult Index()
        {
            var projects = _projectService.GetProjects();
            return View(projects);
        }
    }
}