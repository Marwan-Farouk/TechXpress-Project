using Business.DTOs.Brand;
using Business.Managers.Brand;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModel.Brand;

namespace Presentation.Controllers
{
    public class BrandController : Controller
    {
        private readonly IBrandManager _brandManager;

        public BrandController(IBrandManager brandManager)
        {
            _brandManager = brandManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var brands = await _brandManager.GetAll();
            var viewModels = brands.Select(b => new BrandViewModel
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description
            }).ToList();

            return View(viewModels);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int id)
        {
            var brand = await _brandManager.GetById(id);
            if (brand == null) return NotFound();

            var viewModel = new BrandViewModel
            {
                Id = brand.Id,
                Name = brand.Name,
                Description = brand.Description
            };

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new BrandViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(BrandViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _brandManager.Create(new CreateBrandDto
            {
                Name = model.Name,
                Description = model.Description
            });

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var brand = await _brandManager.GetById(id);
            if (brand == null) return RedirectToAction(nameof(Index));

            var model = new BrandViewModel
            {
                Id = brand.Id,
                Name = brand.Name,
                Description = brand.Description
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(BrandViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _brandManager.Update(new UpdateBrandDto
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description
            });

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _brandManager.Delete(id);
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
