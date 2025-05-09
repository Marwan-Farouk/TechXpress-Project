using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Business.DTOs.Products
{
    public class UpdateProductRequest
    {
        [Required]
        public int Id { get; set; }  

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int BrandId { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public IFormFile? Image { get; set; }
        

        public string? ExistingImage { get; set; }
    }
}

