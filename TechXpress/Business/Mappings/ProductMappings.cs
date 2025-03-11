using Business.DTOs.Products;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mappings
{
    public static class ProductMappings
    { 
        public static Product ToEntity(this UpdateProductDto updateProductDto)
        {
            return new Product
            {
                Id = updateProductDto.Id,
                Name = updateProductDto.Name,
                Description = updateProductDto.Description,
                Price = updateProductDto.Price,
                Stock = updateProductDto.Stock
            };
        }

        public static Product ToEntity(this CreateProductDto createProductDto)
        {
            return new Product
            {
                Id = createProductDto.Id,
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                Stock = createProductDto.Stock,
                Image = createProductDto.Image
            };
        }

        public static List<GetAllProductsDto> ToDto(this List<Product> products)
        {
            return products.Select(prod => new GetAllProductsDto
            {
                Id = prod.Id,
                Name = prod.Name,
                Price = prod.Price,
                CategoryId = prod.CategoryId
            }).ToList();
        }

        public static GetProductByIdDto ToDto(this Product product) 
        { 
            return new GetProductByIdDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Image = product.Image,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                DateAdded = product.DateAdded
            };
        } 
    }
}
