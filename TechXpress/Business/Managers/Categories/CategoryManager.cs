using Business.DTOs.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task CreateCategoryAsync(CreateCategoryDto dto)
        {
            var category = dto.ToEntity();
            //category.Id = await _categoryRepository.GetMaxIdAsync() + 1; // إذا كان لديك هذا الأسلوب
            await _categoryRepository.AddAsync(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _categoryRepository.DeleteAsync(id);
        }

        public dynamic GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<GetAllCategoriesDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.ToDto();
        }

        public async Task<GetCategoryByIdDto> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category?.ToDto(); // تجنب NullReferenceException باستخدام ?
        }

        public async Task UpdateCategoryAsync(UpdateCategoryDto dto)
        {
            var category = dto.ToEntity();
            await _categoryRepository.UpdateAsync(category);
        }
    }
}