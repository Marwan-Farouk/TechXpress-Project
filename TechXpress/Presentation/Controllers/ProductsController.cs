using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Presentation.Models;
using System;
using DataAccess.Entities;
using Business.DTOs.Products;
using DataAccess.Contexts;

namespace Presentation.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            var products = context.Products.OrderByDescending(p => p.Id).ToList();
            return View(products);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateProductDto createproductDetails)
        {
            if (createproductDetails.Image == null)
            {
                ModelState.AddModelError("ImageFile", "The ImageFile is required");
            }
            if (!ModelState.IsValid)
            {
                return View(createproductDetails);
            }
            // Save the image to wwwroot/image
            string newFileNAme = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileNAme += Path.GetExtension(createproductDetails.Image?.FileName);

            string imageFullPath = environment.WebRootPath + "/products/" + newFileNAme;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                createproductDetails.Image?.CopyTo(stream);
            }
            // Save the new Product to the database
            Product newProduct = new Product
            {
                Name = createproductDetails.Name,
                Price = createproductDetails.Price,
                Description = createproductDetails.Description,
                Image = newFileNAme,
                Stock = createproductDetails.Stock,
            };
            context.Products.Add(newProduct);
            context.SaveChanges();
            return RedirectToAction("Index", "Products");
        }
        public IActionResult Edit(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("index", "Products");
            }

            //create a productDetails from the product
            CreateProductDto createproductDetails = new CreateProductDto
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,

                Stock = product.Stock
            };

            ViewData["ImageFileName"] = product.Image;
            ViewData["Id"] = product.Id;
            return View(createproductDetails);
        }
        [HttpPost]
        public IActionResult Edit(int id, CreateProductDto createproductDetails)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("index", "Products");
            }

            if (!ModelState.IsValid)
            {
                ViewData["ImageFileName"] = product.Image;
                ViewData["CreatedAt"] = ((DateTime)product.DateAdded).ToString("MM/dd/yyyy");
                ViewData["Id"] = product.Id;
                return View(createproductDetails);
            }

            product.Name = createproductDetails.Name;
            product.Price = createproductDetails.Price;
            product.Description = createproductDetails.Description;
            product.Stock = createproductDetails.Stock;

            if (createproductDetails.Image != null)
            {
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(createproductDetails.Image.FileName);
                string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    createproductDetails.Image.CopyTo(stream);
                }
                product.Image = newFileName;
            }

            context.Products.Update(product);
            context.SaveChanges();

            return RedirectToAction("Index", "Products");
        }

        public IActionResult Delete(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("index", "Products");
            }
            string FullImagePath = environment.WebRootPath + "/products/" + product.Image;
            System.IO.File.Delete(FullImagePath);

            context.Products.Remove(product);
            context.SaveChanges(true);

            return RedirectToAction("Index", "Products");
        }
        public IActionResult Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return View(new List<Product>());
            }

            var products = context.Products
                .Where(p => p.Name.Contains(query) || p.Description.Contains(query))
                .OrderByDescending(p => p.Id)
                .ToList();

            return View(products);
        }
    }


    public class CreateProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IFormFile Image { get; set; }
        public int Stock { get; set; }
    }
}
