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

        // 📌 List all products
        [HttpGet]
        public IActionResult Index()
        {
            var productDTOs = _productManager.GetAllProducts();
            return View("Index", productDTOs);
        }

        // 📌 Show product details
        [HttpGet]
        public IActionResult Details(int id)
        {
            var product = _productManager.GetProductById(id);
            if (product == null) return NotFound();
            return View("Details", product);
        }

        // 📌 Show create form
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _categoryManager.GetAllCategories();
            ViewBag.Brands = _context.Brands.ToList();
            return View();
        }

        // 📌 Handle create form submission
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

            string? imagePath = null;

            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);
                imagePath = Path.Combine("images/products", uniqueFileName);

                using (var fileStream = new FileStream(Path.Combine(uploadsFolder, uniqueFileName), FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }
            }

            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Stock = model.Stock,
                CategoryId = model.CategoryId,
                BrandId = model.BrandId,
                Image = imagePath,
                DateAdded = DateTime.UtcNow
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // 📌 Show update form
        [HttpGet]
        public IActionResult Update(int id)
        {
            var product = _productManager.GetProductById(id);
            if (product == null) return NotFound();

            ViewBag.Categories = _categoryManager.GetAllCategories();
            ViewBag.Brands = _context.Brands.ToList();

            return View(product);
        }

        // 📌 Handle update form submission
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

        // 📌 Show edit form
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _productManager.GetProductById(id);
            if (product == null) return NotFound();

            var editProductDto = new UpdateProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                BrandId = product.BrandId,
                // Do NOT assign Image here because it's an IFormFile, not a string
            };

            ViewBag.Categories = _categoryManager.GetAllCategories();
            ViewBag.Brands = _context.Brands.ToList();

            return View(editProductDto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UpdateProductDto model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _categoryManager.GetAllCategories();
                ViewBag.Brands = _context.Brands.ToList();
                return View(model);
            }

            var product = _context.Products.Find(model.Id);
            if (product == null) return NotFound();

            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);
                string imagePath = Path.Combine("images/products", uniqueFileName);

                using (var fileStream = new FileStream(Path.Combine(uploadsFolder, uniqueFileName), FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }

                product.Image = imagePath;
            }

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.Stock = model.Stock;
            product.CategoryId = model.CategoryId;
            product.BrandId = model.BrandId;

            _context.Products.Update(product);
            _context.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = model.Id });
        }



        // 📌 Show delete confirmation
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = _productManager.GetProductById(id);
            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection form)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
