using Business.DTOs.Categories;
using Business.DTOs.Products;

namespace Presentation.ViewModel.Category
{
    public class CategoryProductsViewModel
    {
        public GetCategoryByIdDto Category { get; set; }
        public List<GetAllProductsDto> Products { get; set; }
    }
}
