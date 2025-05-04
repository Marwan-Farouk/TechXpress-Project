using Business.DTOs.Products;
using Business.Managers.Brand;
using Business.Managers.Categories;
using Business.Managers.Products;
using DataAccess.Contexts;
using DataAccess.Repositories.BRAND;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PresentationLayer.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductManager _productManager;
        private readonly ICategoryManager _categoryManager;
        private readonly IBrandManager _brandManager;

        public ProductController(IProductManager productManager, ICategoryManager categoryManager, IBrandManager brandManager)
        {
            _productManager = productManager;
            _categoryManager = categoryManager;
            _brandManager = brandManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search, int? categoryId, int? brandId)
        {
            ViewBag.Brands = await _brandManager.GetAllAsync();
            // Get products (already properly awaited)
            var products = await _productManager.GetAllProductsAsync();

            // Apply filters (sync operations are fine here)
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p =>
                    p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    p.Description.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (categoryId.HasValue)
                products = products.Where(p => p.CategoryId == categoryId.Value).ToList();

            if (brandId.HasValue)
                products = products.Where(p => p.BrandId == brandId.Value).ToList();

            // Use the correct method to fetch categories asynchronously
            ViewBag.Categories = await _categoryManager.GetAllCategoriesAsync();
            ViewBag.Brands = _brandManager.GetAllAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Use the correct method to fetch categories asynchronously
            ViewBag.Categories = await _categoryManager.GetAllCategoriesAsync();
            ViewBag.Brands = _brandManager.GetAllAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductRequest model)
        {
            if (!ModelState.IsValid)
            {
                // Use the correct method to fetch categories asynchronously
                ViewBag.Categories = await _categoryManager.GetAllCategoriesAsync();
                ViewBag.Brands = _brandManager.GetAllAsync();

                return View(model);
            }

            await _productManager.CreateProductAsync(new CreateProductDto
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Stock = model.Stock,
                CategoryId = model.CategoryId,
                BrandId = model.BrandId,
                Image = model.Image
            });

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productManager.GetProductByIdAsync(id);
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
                ExistingImage = product.Image
            };

            // Use the correct method to fetch categories asynchronously
            ViewBag.Categories = await _categoryManager.GetAllCategoriesAsync();
            ViewBag.Brands = _brandManager.GetAllAsync();

            return View(editProductDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateProductRequest model)
        {
            if (!ModelState.IsValid)
            {
                // Use the correct method to fetch categories asynchronously
                ViewBag.Categories = await _categoryManager.GetAllCategoriesAsync();
                ViewBag.Brands = _brandManager.GetAllAsync();

                return View(model);
            }

            await _productManager.UpdateProductAsync(new UpdateProductDto
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Stock = model.Stock,
                CategoryId = model.CategoryId,
                BrandId = model.BrandId
            });

            return RedirectToAction(nameof(Index));
        }
    }
}