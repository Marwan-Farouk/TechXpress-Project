using Business.DTOs.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Repositories.CATEGORY;
using Business.Mappings;
namespace Business.Managers.Categories
{
    public class CategoryManager : ICategoryManager
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryManager(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public void CreateCategory(CreateCategoryDto dto)
        {
            var category = dto.ToEntity();
            category.Id = _categoryRepository.GetMaxId() + 1;
            _categoryRepository.Add(category);
        }

        public void DeleteCategory(int id)
        {
            _categoryRepository.Delete(id);
        }

        public List<GetAllCategoriesDto> GetAllCategories()
        {
            var categories = _categoryRepository.GetAll();
            return categories.ToDto();
        }

        public GetCategoryByIdDto GetCategoryById(int id)
        {
            var category = _categoryRepository.GetById(id);
            return category.ToDto();
        }

        public void UpdateCategory(UpdateCategoryDto dto)
        {
            var category = dto.ToEntity();
            _categoryRepository.Update(category);
        }
    }
}
