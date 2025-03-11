using Business.DTOs.Products;
using DataAccess.Entities;
using DataAccess.Repositories.PRODUCT;
namespace Business.Managers
{
    public class ProductManager : IProductManager
    {
        private IProductRepository _productRepository;

        public ProductManager(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void CreateProduct(CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Image = dto.Image,
                Stock = dto.Stock
            };
        }

        public List<GetAllProductsDto> GetAllProducts()
        {
            var productEntities = _productRepository.GetAll();
            var productDtos = productEntities.Select(prod => new GetAllProductsDto
            {
                Id = prod.Id,
                Name = prod.Name,
                Price = prod.Price,
                CategoryId = prod.CategoryId
            }).ToList();
            return productDtos;
        }

        public GetProductByIdDto GetProductById(int id)
        {
            var productEntity = _productRepository.GetById(id);

            if (productEntity == null)
            {
                return null;
            }

            var productDto = new GetProductByIdDto
            {
                //Id = _productRepository.
                Name = productEntity.Name,
                Description = productEntity.Description,
                Price = productEntity.Price,
                Image = productEntity.Image,
                Stock = productEntity.Stock,
                CategoryId = productEntity.CategoryId,
                DateAdded = productEntity.DateAdded
            };
            return productDto;
        }

        public void UpdateProduct(UpdateProductDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
