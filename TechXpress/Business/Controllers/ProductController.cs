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
    [HttpPost]
        // /product/update
        public IActionResult Update(UpdateProductActionRequest request)
        {
            var product = new Product
            { 

            var productDto = request;ToDto();

            _productManager.UpdateProduct(product);

            return RedirectToAction(nameof(Details), new { id = product.Id });
        }