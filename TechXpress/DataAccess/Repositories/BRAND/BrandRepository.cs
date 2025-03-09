using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.BRAND
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ApplicationDbContext _context;

        public BrandRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Brand? GetById(int id)
        {
            return _context.Brands.FirstOrDefault(b => b.Id == id);
        }

        public List<Brand> GetAll()
        {
            return _context.Brands.ToList();
        }

        public void Add(Brand brand)
        {
            if (brand == null)
            {
                throw new ArgumentNullException(nameof(brand));
            }
            _context.Brands.Add(brand);
            _context.SaveChanges();
        }
        public void Update(Brand brand)
        {
            if (brand == null)
            {
                throw new ArgumentNullException(nameof(brand));
            }
            _context.Brands.Update(brand);
            _context.SaveChanges();
        }  
        
        public void Delete(int id)
        {
        var brand = _context.Brands.FirstOrDefault(b => b.Id == id);
            if (brand != null)
            {
                _context.Brands.Remove(brand);
                _context.SaveChanges();
            }
        }
        public List<Brand> SearchByName(string name)
        {
            return _context.Brands.Where(b => b.Name.Contains(name)).ToList();
        }

        public List<Product> GetProductsByBrandId(int brandId)
        {
            return _context.Products
                .Where(p => p.BrandId == brandId)
                .ToList();
        
        }
    }
}
