using Business.DTOs.Products;
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
        private readonly IBrandRepository _brandRepository;

        public ProductController(IProductManager productManager, ICategoryManager categoryManager,IBrandRepository brandRepository)
        {
            _productManager = productManager;
            _categoryManager = categoryManager;
            _brandRepository = brandRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search, int? categoryId, int? brandId)
        {
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

            // 🔥 Critical fix: Properly await all async calls
            ViewBag.Categories = await _categoryManager.GetAllCategoriesAsync();
            ViewBag.Brands = await _brandRepository.GetAllAsync(); // Ensure this exists

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productManager.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            return RedirectToAction("AddToCart", "ShoppingCart", new { productId = id });
        }

        // 📌 Show create form
        
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // 🔥 Critical fix: Add await here
            ViewBag.Categories = await _categoryManager.GetAllCategoriesAsync();
            ViewBag.Brands = await _brandRepository.GetAllAsync();
            return View();
        }

        // 📌 Handle create form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductRequest model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _categoryManager.GetAllCategoriesAsync();
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

        // 📌 Show edit form
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

            ViewBag.Categories = await
                _categoryManager.GetAllCategoriesAsync();
            ViewBag.Brands = await _brandRepository.GetAllAsync();


            return View(editProductDto);
        }

        // 📌 Handle edit form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateProductRequest model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _categoryManager.GetAllCategoriesAsync();
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

            return RedirectToAction(nameof(Details), new { id = model.Id });
        }
    }
}