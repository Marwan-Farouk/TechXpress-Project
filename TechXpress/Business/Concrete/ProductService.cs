using Business.Abstract;
using Business.DTOs.Products;

namespace Business.Concrete;

public class ProductService : IProductService
{
    public List<GetAllProductsDto> GetAll()
    {
        // TODO: Implement actual data retrieval from your data source
        // This is a temporary implementation that returns sample data
        return new List<GetAllProductsDto>
        {
            new GetAllProductsDto
            {
                Id = 1,
                Name = "Sample Product 1",
                Description = "This is a sample product",
                Price = 99.99m,
                Stock = 10,
                BrandId = 1,
                CategoryId = 1,
                Image = "sample1.jpg"
            },
            new GetAllProductsDto
            {
                Id = 2,
                Name = "Sample Product 2",
                Description = "This is another sample product",
                Price = 149.99m,
                Stock = 5,
                BrandId = 1,
                CategoryId = 2,
                Image = "sample2.jpg"
            }
        };
    }
}
