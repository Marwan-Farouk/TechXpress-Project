using Business.DTOs.Categories;
using Business.Managers.Categories;
using Microsoft.AspNetCore.Mvc;

namespace BestStoreMVC.Controllers
{
    public class CategoresController : Controller
    {
        private readonly ICategoryManager _categoryManager;

        public CategoresController(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        public IActionResult Index()
        {
            var categories = _categoryManager.GetAllCategories();
            return View(categories);
        }

        public IActionResult Details(int id)
        {
            var category = _categoryManager.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateCategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryDto);
            }

            _categoryManager.CreateCategory(categoryDto);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var category = _categoryManager.GetCategoryById(id);
            if (category == null)
            {
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(UpdateCategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryDto);
            }

            _categoryManager.UpdateCategory(categoryDto);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            _categoryManager.DeleteCategory(id);
            return RedirectToAction("Index");
        }
    }
}
