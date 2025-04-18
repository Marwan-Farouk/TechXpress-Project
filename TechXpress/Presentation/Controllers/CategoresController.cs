using BestStoreMVC.ViewModels;
using Business.DTOs.Categories;
using Business.Managers.Categories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BestStoreMVC.Controllers
{
    public class CategoresController : Controller
    {
        private readonly ICategoryManager _categoryManager;

        public CategoresController(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        public IActionResult Index(string searchString, string stockFilter)
        {
            // Get all categories from the manager
            var allCategories = _categoryManager.GetAllCategories();

            // Filter by search string
            if (!string.IsNullOrEmpty(searchString))
            {
                allCategories = allCategories
                    .Where(c => c.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Filter by stock status
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

            // Map to ViewModel
            var categories = allCategories
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Stock = c.Stock
                }).ToList();

            // Pass the search and filter values to the view
            ViewData["CurrentFilter"] = searchString;
            ViewData["StockFilter"] = stockFilter;

            return View(categories);
        }

        public IActionResult Details(int id)
        {
            var category = _categoryManager.GetCategoryById(id);
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

        public IActionResult Create()
        {
            return View(new CategoryViewModel());
        }

        [HttpPost]
        public IActionResult Create(CategoryViewModel categoryViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryViewModel);
            }

            _categoryManager.CreateCategory(new CreateCategoryDto
            {
                Name = categoryViewModel.Name,
                Description = categoryViewModel.Description,
                Stock = categoryViewModel.Stock
            });

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _categoryManager.GetCategoryById(id);
            if (category == null)
            {
                return RedirectToAction("Index");
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

        [HttpPost]
        public IActionResult Edit(CategoryViewModel categoryViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryViewModel);
            }

            _categoryManager.UpdateCategory(new UpdateCategoryDto
            {
                Id = categoryViewModel.Id,
                Name = categoryViewModel.Name,
                Description = categoryViewModel.Description,
                Stock = categoryViewModel.Stock
            });

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            _categoryManager.DeleteCategory(id);
            return RedirectToAction("Index");
        }
    }
}