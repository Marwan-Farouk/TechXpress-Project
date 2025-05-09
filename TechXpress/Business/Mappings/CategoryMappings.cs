using Business.DTOs.Categories;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mappings
{
    public static class CategoryMappings
    {
        public static Category ToEntity(this CreateCategoryDto dto)
        {
            return new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                Stock = dto.Stock
            };
        }
        public static Category ToEntity(this UpdateCategoryDto dto)
        {
            return new Category
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Stock = dto.Stock
            };
        }
        public static List<GetAllCategoriesDto>ToDto(this List<Category> categories)
        {
            return categories.Select(category => new GetAllCategoriesDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Stock = category.Stock
            }).ToList();
        }

        public static GetCategoryByIdDto ToDto(this Category category)
        {
            return new GetCategoryByIdDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Stock = category.Stock
            };
        }
    }
}
