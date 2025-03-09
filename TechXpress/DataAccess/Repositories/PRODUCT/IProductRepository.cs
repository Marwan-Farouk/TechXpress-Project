using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.PRODUCT
{
    interface IProductRepository
    {
        Product GetById(int id);
        List<Product> GetAll();
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
        List<Product> SearchByName(string name);
        List<Product> GetProductsByCategory(int categoryId);
        List<Product> GetProductsByBrand(int brandId);
    }
}
