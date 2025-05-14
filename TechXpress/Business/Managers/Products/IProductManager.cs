using Business.DTOs.Products;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Managers.Products
{

    public interface IProductManager
    {
        Task<List<GetAllProductsDto>> GetAllProductsAsync();
        Task<GetProductByIdDto?> GetProductByIdAsync(int id);
        Task CreateProductAsync(CreateProductDto dto);
        Task UpdateProductAsync(UpdateProductDto dto);
        Task DeleteProductAsync(int id);
        Task<dynamic> GetAllBrandsAsync();
        Task<int> GetProductStockAsync(int id);
        Task<List<GetAllProductsDto>> GetProductsByCategoryAsync(int categoryId);
    }
}
