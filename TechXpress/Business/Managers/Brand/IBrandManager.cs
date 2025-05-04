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
       public BrandDto? GetById(int id);
        public List<BrandDto> GetAll();
        public int Create(CreateBrandDto dto);
        public void Update(UpdateBrandDto dto);
        public void Delete(int id);
    }
}
