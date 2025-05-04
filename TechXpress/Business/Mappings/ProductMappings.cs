using Business.DTOs.Products;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace Business.Mappings
{
    public static class ProductMappings
    {
        public static Product ToEntity(this CreateProductDto createProductDto)
        {
            return new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                Stock = createProductDto.Stock,
                Image = createProductDto.Image != null ? SaveImage(createProductDto.Image) : null,
                BrandId = createProductDto.BrandId,
                CategoryId = createProductDto.CategoryId,
                DateAdded = createProductDto.DateAdded
            };
        }

        public static Product ToEntity(this UpdateProductDto updateProductDto)
        {
            return new Product
            {
                Id = updateProductDto.Id,
                Name = updateProductDto.Name,
                Description = updateProductDto.Description,
                Price = updateProductDto.Price,
                Stock = updateProductDto.Stock,
                BrandId = updateProductDto.BrandId,
                CategoryId = updateProductDto.CategoryId,
                // Image here is the string path/name, not an IFormFile
                Image = updateProductDto.Image
            };
        }

        public static GetProductByIdDto? ToDto(this Product product)
        {
            if (product == null) return null;
            return new GetProductByIdDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Image = product.Image,
                Stock = product.Stock,
                DateAdded = product.DateAdded,
                BrandId = product.BrandId,
                CategoryId = product.CategoryId
            };
        }

        public static List<GetAllProductsDto> ToDto(this List<Product> products)
        {
            return products.Select(product => new GetAllProductsDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Image = product.Image, Stock = product.Stock,
                BrandId = product.BrandId,
                CategoryId = product.CategoryId,
                DateAdded = product.DateAdded,
                Description = product.Description,
            }).ToList();
        }

        private static string SaveImage(IFormFile imageFile)
        {
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
            Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
            }

            return Path.Combine("images/products", uniqueFileName);
        }
    }
}
