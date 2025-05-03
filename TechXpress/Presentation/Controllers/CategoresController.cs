using BestStoreMVC.ViewModels;
using Business.DTOs.Categories;
using Business.Managers.Categories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestStoreMVC.Controllers
{
    public class CategoresController : Controller
    {
        private readonly ICategoryManager _categoryManager;

        public CategoresController(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }
        [HttpGet]
        // GET: /Categores/Index
        public async Task<IActionResult> Index(string searchString, string stockFilter)
        {
            var allCategories = await _categoryManager.GetAllCategoriesAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                allCategories = allCategories
                    .Where(c => c.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(stockFilter))
            {
                switch (stockFilter)
                {
                    case "inStock":
                        allCategories = allCategories.Where(c => c.Stock > 0).ToList();
                        break;
                    case "outOfStock":
                        allCategories = allCategories.Where(c => c.Stock == 0).ToList();
                        break;
                }
            }

            var categories = allCategories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Stock = c.Stock
            }).ToList();

            ViewData["CurrentFilter"] = searchString;
            ViewData["StockFilter"] = stockFilter;

            return View(categories);
        }
        [HttpGet]
        // GET: /Categores/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryManager.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();

            var viewModel = new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Stock = category.Stock
            };

            return View(viewModel);
        }
        [HttpGet]
        // GET: /Categores/Create
        public IActionResult Create()
        {
            return View(new CategoryViewModel());
        }

        // POST: /Categores/Create
        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel categoryViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryViewModel);
            }

            await _categoryManager.CreateCategoryAsync(new CreateCategoryDto
            {
                Name = categoryViewModel.Name,
                Description = categoryViewModel.Description,
                Stock = categoryViewModel.Stock
            });

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        // GET: /Categores/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryManager.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Stock = category.Stock
            };

            return View(viewModel);
        }

        // POST: /Categores/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(CategoryViewModel categoryViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryViewModel);
            }

            await _categoryManager.UpdateCategoryAsync(new UpdateCategoryDto
            {
                Id = categoryViewModel.Id,
                Name = categoryViewModel.Name,
                Description = categoryViewModel.Description,
                Stock = categoryViewModel.Stock
            });

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        // GET: /Categores/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryManager.DeleteCategoryAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}