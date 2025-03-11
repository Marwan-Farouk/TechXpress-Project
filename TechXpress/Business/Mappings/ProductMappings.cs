using BusinessLayer.DTOs.Products;
using PresentationLayer.ActionRequests.Products;

namespace PresentationLayer.Mappings
{
    public static class ProductMappings
    {

        public static Product ToDto(this UpdateProductActionRequest request)
            => new Product
            {
                Id = dto.Id,
                Name = dto.Name,
                Price = dto.Price,
                Description = dto.Description
            };
    }
}