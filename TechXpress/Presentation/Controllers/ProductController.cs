using Business.DTOs.Products;
using Business.Managers.Products;
using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductManager _productManager; //= new ProductManager(); // Composition ===> Tightly Coupled
        private readonly FileService _fileService = new FileService();

        public ProductController(IProductManager productManager)
        {
            _productManager = productManager;
        }
        // 🚀 /product/index
        [HttpGet]
        public IActionResult Index()
        {
            // 1- Catch Request

            // 2- Call Model    ---->  Model returns data
            var productDTOs = _productManager.GetAllProducts();

            //var productVMS = productDTOs
            //    .Select(dto => new GetAllProductsVM
            //    {
            //        Id = dto.Id,
            //        Name = dto.Name,
            //        Price = dto.Price,
            //        Image = dto.Image,
            //    })
            //    .ToList();

            // 3- send data to view
            //return View();
            //return View("Index");
            return View("Index", productDTOs);
        }
        [HttpGet]

        //          /product/details/:id      ✅ Path Variable
        //          /product/details?id=3     ✅ Query Parameter
        public IActionResult Details(int id)
        {
            var product = _productManager.GetProductById(id);

            if(product == null)
            {
                return NotFound();
            }

            //var productVM = new GetProductByIdVM
            //{
            //    Id = product.Id,
            //    Name = product.Name,
            //    Price = product.Price,
            //    Image = product.Image,
            //    Description = product.Description,
            //};

            return View("Details", product);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }
        [HttpPost]
        public IActionResult Create(CreateProductDto request)
        {
            // 1- Upload Image
            //var uniqueFileName = _fileService.UploadFile(request.Image, "Images");
            
            // 2- new product object
            //var productDto = new CreateProductDto
            //{
            //    //Id = _productManager.GetMaxId() + 1,
            //    Name = request.Name,
            //    Price = request.Price,
            //    Description = request.Description,
            //    //Image = uniqueFileName
            //};

            // 3- add to the products list
            _productManager.CreateProduct(request);

            // 4- Redirection

            //return RedirectToAction("Index");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        //   /product/update/:id
        public IActionResult Update(int id)
        {
            var product = _productManager.GetProductById(id);

            //var productDto = new UpdateProductDto
            //{
            //    Id = product.Id,
            //    Name = product.Name,
            //    Price = product.Price,
            //    Description = product.Description,
            //};

            return View(product);
        }

        [HttpPost]
        //   /product/update
        public IActionResult Update(UpdateProductDto request)
        {

            //var productDto = request.ToDto();

            _productManager.UpdateProduct(request);

            return RedirectToAction(nameof(Details), new { id = request.Id });
        }
    }
}
