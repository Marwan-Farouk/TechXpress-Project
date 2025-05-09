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
       public Task<BrandDto?> GetById(int id);
        public Task<List<BrandDto>> GetAll();
        public Task<int> Create(CreateBrandDto dto);
        public Task Update(UpdateBrandDto dto);
        public Task Delete(int id);
    }
}
