using Business.DTOs.Products;
using Business.Managers.Brand;
using Business.Managers.Categories;
using Business.Managers.Products;
using DataAccess.Contexts;
using DataAccess.Repositories.BRAND;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Business.DTOs.Categories;
using Microsoft.AspNetCore.Authorization;
using Presentation.ViewModel.Category;

namespace PresentationLayer.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductManager _productManager;
        private readonly ICategoryManager _categoryManager;
        private readonly IBrandManager _brandManager;

        public ProductController(IProductManager productManager, ICategoryManager categoryManager,IBrandManager brandManager)
        {
            _productManager = productManager;
            _categoryManager = categoryManager;
            _brandManager = brandManager;
        }

        [HttpGet]

        public async Task<IActionResult> Index(string search, int? categoryId, int? brandId, decimal? minPrice, decimal? maxPrice, string sortBy, int page = 1)
        {

            var products = await _productManager.GetAllProductsAsync();

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

            if (minPrice.HasValue)
                products = products.Where(p => p.Price >= minPrice.Value).ToList();
            if (maxPrice.HasValue)
                products = products.Where(p => p.Price <= maxPrice.Value).ToList();


            switch (sortBy)
            {
                case "price-asc":
                    products = products.OrderBy(p => p.Price).ToList();
                    break;
                case "price-desc":
                    products = products.OrderByDescending(p => p.Price).ToList();
                    break;
                case "name-asc":
                    products = products.OrderBy(p => p.Name).ToList();
                    break;
                case "name-desc":
                    products = products.OrderByDescending(p => p.Name).ToList();
                    break;
                default:
                    products = products.OrderByDescending(p => p.DateAdded).ToList();
                    break;
            }

            ViewBag.Categories = await _categoryManager.GetAllCategoriesAsync();
            ViewBag.Brands = await _brandManager.GetAll();

            // Pagination
            const int pageSize = 10;
            var totalItems = products.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var pagedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;

            ViewBag.Categories = await _categoryManager.GetAllCategoriesAsync();
            ViewBag.Brands = await _brandManager.GetAll();
            ViewBag.Search = search;
            ViewBag.CategoryId = categoryId;
            ViewBag.BrandId = brandId;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.SortBy = sortBy;
            return View(pagedProducts);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productManager.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            ViewBag.Category = await _categoryManager.GetCategoryByIdAsync(product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Customer")]
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            return RedirectToAction("AddToCart", "ShoppingCart", new { id, quantity });
        }

        // 📌 Show create form
        [HttpGet]
        [Authorize(Roles = "Seller, Admin")]
        public async Task<IActionResult> Create()
        {

            ViewBag.Categories = await _categoryManager.GetAllCategoriesAsync();
            ViewBag.Brands = await _brandManager.GetAll();

            return View();
        }

        // 📌 Handle create form submission
        [HttpPost]
        [Authorize(Roles = "Seller, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductRequest model)
        {
            if (!ModelState.IsValid)
            {
            
                ViewBag.Categories = await _categoryManager.GetAllCategoriesAsync();
                ViewBag.Brands = await _brandManager.GetAll();

                ViewBag.Categories =await _categoryManager.GetAllCategoriesAsync();
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

            await _categoryManager.IncrementCategoryStockAsync(model.CategoryId, model.Stock);


            return RedirectToAction("Index");
        }

        // 📌 Show edit form
        [HttpGet]
        [Authorize(Roles = "Seller, Admin")]
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

            ViewBag.Categories = await _categoryManager.GetAllCategoriesAsync();
            ViewBag.Brands = await _brandManager.GetAll();

            return View(editProductDto);
        }

        // 📌 Handle edit form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Seller, Admin")]
        public async Task<IActionResult> Edit(UpdateProductRequest model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryManager.GetAllCategoriesAsync();
                ViewBag.Brands = await _brandManager.GetAll();
                return View(model);
            }

            // Get the original product to check if category changed
            var originalProduct = await _productManager.GetProductByIdAsync(model.Id);
            if (originalProduct == null)
            {
                return NotFound();
            }
        
            // Check if category changed and store original values
            bool categoryChanged = originalProduct.CategoryId != model.CategoryId;
            int oldCategoryId = originalProduct.CategoryId;
            int oldStock = originalProduct.Stock;
        
            // Update the product
            await _productManager.UpdateProductAsync(new UpdateProductDto
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Stock = model.Stock,
                CategoryId = model.CategoryId,
                BrandId = model.BrandId,
            });
        
            // If category changed, update both categories' stock counts
            if (categoryChanged)
            {
                // Decrement stock from old category
                await _categoryManager.DecrementCategoryStockAsync(oldCategoryId, oldStock);
                
                // Increment stock for new category
                await _categoryManager.IncrementCategoryStockAsync(model.CategoryId, model.Stock);
            }
            // If only stock changed (not category), update the difference
            else if (oldStock != model.Stock)
            {
                int stockDifference = model.Stock - oldStock;
                if (stockDifference > 0)
                {
                    await _categoryManager.IncrementCategoryStockAsync(model.CategoryId, stockDifference);
                }
                else if (stockDifference < 0)
                {
                    await _categoryManager.DecrementCategoryStockAsync(model.CategoryId, Math.Abs(stockDifference));
                }
            }

            return RedirectToAction(nameof(Details), new { id = model.Id });
        }
        public async Task<IActionResult> CategoryProducts(int categoryId)
        {
            var products = await _productManager.GetProductsByCategoryAsync(categoryId);
            var category = await _categoryManager.GetCategoryByIdAsync(categoryId);

            if (category == null)
                return NotFound();

            var viewModel = new CategoryProductsViewModel
            {
                Category = category,
                Products = products
            };

            return View(viewModel);
        }
        public async Task<IActionResult> Delete(int id)
        {
            await _productManager.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}