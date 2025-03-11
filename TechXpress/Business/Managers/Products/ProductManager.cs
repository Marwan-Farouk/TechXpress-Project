using Business.DTOs.Products;
using Business.Mappings;
using DataAccess.Repositories.PRODUCT;
namespace Business.Managers.Products
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
            var product = dto.ToEntity();
         
            product.Id = _productRepository.GetMaxId() + 1;

            _productRepository.Add(product);
        }

        public List<GetAllProductsDto> GetAllProducts()
        {
            var productEntities = _productRepository.GetAll();

            var productDtos = productEntities.ToDto();

            return productDtos;
        }

        public GetProductByIdDto GetProductById(int id)
        {
            var productEntity = _productRepository.GetById(id);

            if (productEntity == null)
            {
                return null;
            }



            var productDto = productEntity.ToDto();

            return productDto;
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
    }
}
