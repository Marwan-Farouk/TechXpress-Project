using DataAccess.Contexts;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.PRODUCT
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Product GetById(int id)
        {
            return _context.Products.FirstOrDefault(x => x.Id == id);
        }
        public List<Product> GetAll()
        {
            return _context.Products.ToList();
        }
        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }
        public void Update(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var prod = _context.Products.FirstOrDefault(x => x.Id == id);
            if (prod is not null)
            {
            _context.Products.Remove(prod);
            _context.SaveChanges();
            }
        }
        public List<Product> SearchByName(string name)
        {
            return _context.Products.Where(x => x.Name.Contains(name)).ToList();
        }
        public List<Product> GetProductsByCategory(int id)
        {
            return _context.Products.Where(x => x.CategoryId == id).ToList();
        }
        public List<Product> GetProductsByBrand(int id)
        {
            return _context.Products.Where(x => x.BrandId == id).ToList();
        }

        public int GetMaxId()
        {
            return _context.Products.Max(p => p.Id);
        }
    }
}
