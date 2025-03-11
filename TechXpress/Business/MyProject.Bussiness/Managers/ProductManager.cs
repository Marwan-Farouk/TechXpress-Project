using DataAccessLayer.Repositories;

namespace BusinessLayer.Managers
{
    public class ProductManager
    {
        private readonly ProductRepository _productRepository = new ProductRepository();

        public List<Product> GetAll()
        {
            // Business Logic
            var productEntities = _productRepository.GetAll();
            return productEntities;
        }

        public Product? GetById(int id)
        {
            return _productRepository.GetById(id);
        }

        public void CreateProduct(Product product)
        {
            _productRepository.CreateProduct(product);
        }

        public void UpdateProduct(Product product)
        {
            _productRepository.UpdateProduct(product);
        }
    }
    public GetProductByIdDto GetById(int id)
        {
            var product = _productRepository.GetById(id);

            var productDto = new GetProductByIdDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Image = product.Image,
                Description = product.Description
            };

            return productDto;
        }