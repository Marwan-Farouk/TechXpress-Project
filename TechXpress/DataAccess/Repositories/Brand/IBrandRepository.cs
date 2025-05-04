using DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories.BRAND
{
    public interface IBrandRepository
    {
        Task<Brand?> GetByIdAsync(int id);
        Task<IEnumerable<Brand>> GetAllAsync();
        Task AddAsync(Brand brand);
        Task UpdateAsync(Brand brand);
        Task DeleteAsync(int id);
        Task<IEnumerable<Brand>> SearchByNameAsync(string name);
        Task<IEnumerable<Product>> GetProductsByBrandIdAsync(int brandId);
    }
}