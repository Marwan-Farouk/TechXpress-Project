using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Presentation.Models;
using Presentation.Services;
using System;

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
        public IActionResult Create(ProductDetails productDetails)
        {
            if (productDetails.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The ImageFile is required");
            }
            if (!ModelState.IsValid)
            {
                return View(productDetails);
            }
            // Save the image to wwwroot/image
            string newFileNAme = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileNAme += Path.GetExtension(productDetails.ImageFile!.FileName);

            string imageFullPath = environment.WebRootPath + "/products/" + newFileNAme;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                productDetails.ImageFile.CopyTo(stream);
            }
            // Save the new Product to the database
            Product newProduct = new Product
            {
                Name = productDetails.Name,
                Price = productDetails.Price,
                Description = productDetails.Description,
                Category = productDetails.Category,
                Brand = productDetails.Brand,
                ImageFileName = newFileNAme,
                CreatedAt = DateTime.Now
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
            ProductDetails productDetails = new ProductDetails
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Category = product.Category,
                Brand = product.Brand
            };

            ViewData["ImageFileName"] = product.ImageFileName;
            ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");
            ViewData["Id"] = product.Id;
            return View(productDetails);

        }
        [HttpPost]
        public IActionResult Edit(int id, ProductDetails productDetails)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("index", "Products");
            }
            if (!ModelState.IsValid)
            {
                ViewData["ImageFileName"] = product.ImageFileName;
                ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");
                ViewData["Id"] = product.Id;
                return View(productDetails);
            }
            // Save the image to wwwroot/image
            string newFileNAme = product.ImageFileName;
            if (productDetails.ImageFile != null)
            {
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileNAme += Path.GetExtension(productDetails.ImageFile!.FileName);
                string imageFullPath = environment.WebRootPath + "/products/" + newFileNAme;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    productDetails.ImageFile.CopyTo(stream);
                }
                //delete the old image
                string oldImagePath = environment.WebRootPath + "/products/" + product.ImageFileName;
                System.IO.File.Delete(oldImagePath);
            }
            // Save the new Product to the database
            product.Name = productDetails.Name;
            product.Price = productDetails.Price;
            product.Description = productDetails.Description;
            product.Category = productDetails.Category;
            product.Brand = productDetails.Brand;
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
            string FullImagePath = environment.WebRootPath + "/products/" + product.ImageFileName;
            System.IO.File.Delete(FullImagePath);

            context.Products.Remove(product);
            context.SaveChanges(true);

            return RedirectToAction("Index", "Products");
        }
    }
}
