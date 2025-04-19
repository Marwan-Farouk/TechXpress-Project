using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task AddAsync(Product product)
        {
            product.BrandId = 1; // to be updated
            product.CategoryId = 2; // to be updated
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var prod = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (prod is not null)
            {
                _context.Products.Remove(prod);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Product>> SearchByNameAsync(string name)
        {
            return await _context.Products.Where(x => x.Name.Contains(name)).ToListAsync();
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(int id)
        {
            return await _context.Products.Where(x => x.CategoryId == id).ToListAsync();
        }

        public async Task<List<Product>> GetProductsByBrandAsync(int id)
        {
            return await _context.Products.Where(x => x.BrandId == id).ToListAsync();
        }

        public async Task<int> GetMaxIdAsync()
        {
            if (!await _context.Products.AnyAsync()) return 0;
            return await _context.Products.MaxAsync(p => p.Id);
        }
    }
}
