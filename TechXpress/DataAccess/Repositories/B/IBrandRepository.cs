using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.BRAND
{
    public interface IBrandRepository
    {
        Brand? GetById(int id);
        List<Brand> GetAll();
        void Add(Brand brand);
        void Update(Brand brand);
        void Delete(int id);
        List<Brand> SearchByName(string name);
        List<Product> GetProductsByBrandId(int brandId);

    }
}
