using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Products
{
    public class UpdateProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        // public IFormFile? Image { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        // This is the string path/name of the image file, not the actual file
        public string Image { get; set; }
    }
}
