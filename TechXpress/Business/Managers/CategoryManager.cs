using MyProject.DataAccess.DTOs;
using System.Collections.Generic;

namespace MyProject.Bussiness.Managers
{
    public class CategoryManager : ICategoryManager
    {
        private readonly List<CategoryDTO> _categories = new();

        public List<CategoryDTO> GetAllCategories()
        {
            return _categories;
        }

        public CategoryDTO GetCategoryById(int id)
        {
            return _categories.Find(c => c.Id == id);
        }

        public void AddCategory(CategoryDTO category)
        {
            _categories.Add(category);
        }

        public void UpdateCategory(CategoryDTO category)
        {
            var existingCategory = _categories.Find(c => c.Id == category.Id);
            if (existingCategory != null)
            {
                existingCategory.Name = category.Name;
            }
        }

        public void DeleteCategory(int id)
        {
            var category = _categories.Find(c => c.Id == id);
            if (category != null)
            {
                _categories.Remove(category);
            }
        }
    }
}