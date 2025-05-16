using Business.DTOs.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Managers.Categories
{
    public interface ICategoryManager
    {
        Task<List<GetAllCategoriesDto>> GetAllCategoriesAsync();
        Task<GetCategoryByIdDto> GetCategoryByIdAsync(int id);
        Task CreateCategoryAsync(CreateCategoryDto dto);
        Task UpdateCategoryAsync(UpdateCategoryDto dto);
        Task DeleteCategoryAsync(int id);
        dynamic GetAllAsync();
        Task IncrementCategoryStockAsync(int categoryId, int stockToAdd);
        Task DecrementCategoryStockAsync(int categoryId, int stockToRemove);

    }
}