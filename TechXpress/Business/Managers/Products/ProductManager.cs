using Business.DTOs.Products;
using Business.Mappings;
using DataAccess.Repositories.PRODUCT;
using System.Collections.Generic;

namespace Business.Managers.Products
{
    public class ProductManager : IProductManager
    {
        private readonly IProductRepository _productRepository;

        public ProductManager(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void CreateProduct(CreateProductDto dto)
        {
            var product = dto.ToEntity();
            _productRepository.Add(product);
        }

        public List<GetAllProductsDto> GetAllProducts()
        {
            var productEntities = _productRepository.GetAll();
            return productEntities.ToDto(); // ✅ Correctly calls ToDto()
        }

        public GetProductByIdDto GetProductById(int id)
        {
            var productEntity = _productRepository.GetById(id);
            return productEntity?.ToDto(); // ✅ Handles null case properly
        }

        public void UpdateProduct(UpdateProductDto dto)
        {
            var product = dto.ToEntity();
            _productRepository.Update(product);
        }

        public void DeleteProduct(int id)
        {
            _productRepository.Delete(id);
        }

        public dynamic GetAllBrands()
        {
            throw new NotImplementedException();
        }
    }
}
