using Business.DTOs.Products;

namespace Business.Abstract;

public interface IProductService
{
    List<GetAllProductsDto> GetAll();
}
