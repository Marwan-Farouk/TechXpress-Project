using Business.DTOs.Brand;
using Business.Managers.Brand;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class BrandController : Controller
    {
        private readonly IBrandManager _brandManager;

        public BrandController(IBrandManager brandManager)
        {
            _brandManager = brandManager;
        }

       
        // Updated method in BrandController
        public async Task<IActionResult> Index()
        {
            var brands = await _brandManager.GetAllAsync() as List<BrandDto>; // Corrected type to List<BrandDto>
            if (brands == null)
            {
                return View(new List<BrandDetailsDto>()); // Handle null case gracefully
            }

            var vm = brands.Select(b => new BrandDetailsDto
            {
                Id = b.Id,
                Name = b.Name
            }).ToList();

            return View(vm);
        }



        // GET: /Brand/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Brand/Create
        [HttpPost]
        public async Task<IActionResult> Create(CreateBrandDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _brandManager.CreateAsync(dto.Name);
            return RedirectToAction("Index");
        }
    }
}
