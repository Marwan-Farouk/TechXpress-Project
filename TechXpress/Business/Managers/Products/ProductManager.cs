using Business.DTOs.Categories;
using Business.DTOs.Products;
using Business.Mappings;
using DataAccess.Entities;
using DataAccess.Repositories.PRODUCT;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Managers.Products
{
    public class ProductManager : IProductManager
    {
        private readonly IProductRepository _productRepository;

        public ProductManager(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task CreateProductAsync(CreateProductDto dto)
        {
            var product = dto.ToEntity();
            await _productRepository.AddAsync(product);
        }

        public async Task<List<GetAllProductsDto>> GetAllProductsAsync()
        {
            var productEntities = await _productRepository.GetAllAsync();
            return productEntities.ToDto(); // ✅ Correctly calls ToDto()
        }

        public async Task<GetProductByIdDto?> GetProductByIdAsync(int id)
        {
            var productEntity = await _productRepository.GetByIdAsync(id);
            return productEntity?.ToDto(); // ✅ Handles null case properly
        }

        public async Task UpdateProductAsync(UpdateProductDto dto)
        {
            var product = dto.ToEntity();
            
            // If we're getting a new image path, use it; otherwise we'll preserve the existing one in repository
            // Product repo will handle keeping the old image path if no new one is provided
            
            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteAsync(id);
        }

        public async Task<dynamic> GetAllBrandsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetProductStockAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product?.Stock ?? 0;
        }
        public async Task<List<GetAllProductsDto>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _productRepository.GetAllAsync();

            var result = products.Select(p => new GetAllProductsDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Image = p.Image,
                Stock = p.Stock,
                CategoryId = p.CategoryId,
                BrandId = p.BrandId,
                DateAdded = p.DateAdded,


            }).ToList();

            return result;
        }

    }
}
