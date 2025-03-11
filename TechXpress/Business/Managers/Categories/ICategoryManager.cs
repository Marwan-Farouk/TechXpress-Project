using Business.DTOs.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Managers.Categories
{
    public interface ICategoryManager
    {
        public List<GetAllCategoriesDto> GetAllCategories();
        public GetCategoryByIdDto GetCategoryById(int id);
        public void CreateCategory(CreateCategoryDto dto);
        public void UpdateCategory(UpdateCategoryDto dto);
        public void DeleteCategory(int id);
    }
}
