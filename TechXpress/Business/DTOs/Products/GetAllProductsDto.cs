﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Products
{
    public class GetAllProductsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int Stock { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

    }
}
