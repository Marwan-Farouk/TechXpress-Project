using Business.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Managers
{
    public interface IProductManager
    {
        List<GetAllProductsDto> GetAllProducts();
        GetProductByIdDto GetProductById(int id);
        void CreateProduct(CreateProductDto dto);
        void UpdateProduct(UpdateProductDto dto);
    }
}
