using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories.CATEGORY
{
    public interface ICategoryRepository
    {
        Task<Category> GetByIdAsync(int id);
        Task<List<Category>> GetAllAsync();
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
        Task<List<Category>> SearchByNameAsync(string name);
        Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId);
        Task<int> GetMaxIdAsync();
    }
}