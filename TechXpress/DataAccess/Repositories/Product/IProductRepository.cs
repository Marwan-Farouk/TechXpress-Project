using DataAccess.Entities;

namespace DataAccess.Repositories.PRODUCT
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(int id);
        Task<List<Product>> GetAllAsync();
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<List<Product>> SearchByNameAsync(string name);
        Task<List<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<List<Product>> GetProductsByBrandAsync(int brandId);
        Task<int> GetMaxIdAsync();
    }
}
