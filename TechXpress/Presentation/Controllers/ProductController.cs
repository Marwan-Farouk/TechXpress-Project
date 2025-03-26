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
        [HttpGet]
        public IActionResult Index(string search, int? categoryId, int? brandId)
        {
            var products = _productManager.GetAllProducts();

            // 🔍 Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                               p.Description.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // 🔍 Apply category filter
            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value).ToList();
            }

            // 🔍 Apply brand filter
            if (brandId.HasValue)
            {
                products = products.Where(p => p.BrandId == brandId.Value).ToList();
            }

            ViewBag.Categories = _categoryManager.GetAllCategories();
            ViewBag.Brands = _context.Brands.ToList();

            return View(products);
        }
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
        
        public IActionResult Create(CreateProductRequest model)
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

            var editProductDto = new UpdateProductRequest
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                BrandId = product.BrandId,
                ExistingImage = product.Image // ✅ Pass current image path
            };

            ViewBag.Categories = _categoryManager.GetAllCategories();
            ViewBag.Brands = _context.Brands.ToList();

            return View(editProductDto);
        }



       [HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Edit(UpdateProductRequest model)
{
    if (!ModelState.IsValid)
    {
        ViewBag.Categories = _categoryManager.GetAllCategories();
        ViewBag.Brands = _context.Brands.ToList();
        return View(model);
    }

    var product = _context.Products.Find(model.Id);
    if (product == null) return NotFound();

    // ✅ Delete old image if a new one is uploaded
    if (model.Image != null)
    {
        if (!string.IsNullOrEmpty(product.Image))
        {
            string oldImagePath = Path.Combine("wwwroot", product.Image);
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
        }

        // ✅ Save new image
        string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);
        string imagePath = Path.Combine("images/products", uniqueFileName);
        string fullPath = Path.Combine("wwwroot", imagePath);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            model.Image.CopyTo(stream);
        }

        product.Image = imagePath; // ✅ Update new image path
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
