using Business.DTOs.Products;
using Business.Managers.Categories;
using Business.Managers.Products;
using Business.Services;
using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PresentationLayer.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductManager _productManager;
        private readonly ICategoryManager _categoryManager;
        private readonly ApplicationDbContext _context;
        private readonly FileService _fileService = new FileService();

        public ProductController(IProductManager productManager, ICategoryManager categoryManager, ApplicationDbContext context)
        {
            _productManager = productManager;
            _categoryManager = categoryManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var productDTOs = _productManager.GetAllProducts();
            return View("Index", productDTOs);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var product = _productManager.GetProductById(id);
            if (product == null) return NotFound();
            return View("Details", product);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _categoryManager.GetAllCategories();
            ViewBag.Brands = _context.Brands.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateProductDto model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _categoryManager.GetAllCategories();
                ViewBag.Brands = _context.Brands.ToList();
                return View(model);
            }

            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Stock = model.Stock,
                CategoryId = model.CategoryId,
                BrandId = model.BrandId
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var product = _productManager.GetProductById(id);
            if (product == null) return NotFound();

            ViewBag.Categories = _categoryManager.GetAllCategories();
            ViewBag.Brands = _context.Brands.ToList();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(UpdateProductDto request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _categoryManager.GetAllCategories();
                ViewBag.Brands = _context.Brands.ToList();
                return View(request);
            }

            _productManager.UpdateProduct(request);
            return RedirectToAction(nameof(Details), new { id = request.Id });
        }
    }
}
