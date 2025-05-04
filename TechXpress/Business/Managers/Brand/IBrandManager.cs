using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.DTOs.Brand;
using DataAccess.Entities;

namespace Business.Managers.Brand
{
    public interface IBrandManager
    {
        Task<BrandDto?> GetByIdAsync(int id); // Updated to async
        Task<List<BrandDto>> GetAllAsync();  // Added missing async method
        Task<int> CreateAsync(CreateBrandDto dto); // Updated to async
        Task UpdateAsync(UpdateBrandDto dto); // Updated to async
        Task DeleteAsync(int id); // Updated to async
        Task CreateAsync(string name);
    }
}
