using DataAccess.Entities;

namespace DataAccess.Repositories.PRODUCT
{
    public interface IProductRepository
    {
        Product GetById(int id);
        List<Product> GetAll();
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
        List<Product> SearchByName(string name);
        List<Product> GetProductsByCategory(int categoryId);
        List<Product> GetProductsByBrand(int brandId);
        public int GetMaxId();

    }
}
