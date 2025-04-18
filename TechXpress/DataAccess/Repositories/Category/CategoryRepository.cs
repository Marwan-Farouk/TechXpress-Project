using DataAccess.Contexts;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.CATEGORY
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Category? GetById(int id)
        {
            return _context.Categories.FirstOrDefault(b => b.Id == id);
        }
         public List<Category> GetAll()
        {
            return _context.Categories.ToList();
        }
        public void Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var cat = _context.Categories.Find(id);
            if (cat is not null)
            {
            _context.Categories.Remove(cat);
            _context.SaveChanges();

            } 
        }
        public List<Category> SearchByName(string name)
        {
            return _context.Categories.Where(b => b.Name.Contains(name)).ToList();
        }

        public List<Product> GetProductsByCategoryId(int categoryId)
        {
            return _context.Products.Where(p => p.CategoryId == categoryId).ToList();
        }
        public int GetMaxId()
        {
            if (_context.Categories.ToList().Count == 0) return 0;
            return _context.Categories.Max(c => c.Id);
        }
    }
}
