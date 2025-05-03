using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.DTOs.Brand;
using DataAccess.Entities;
using DataAccess.Contexts;
using DataAccess.Repositories.BRAND;

namespace Business.Managers.Brand
{
    public class BrandManager : IBrandManager
    {
        private readonly IBrandRepository _brandRepository;

        public BrandManager(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public BrandDto? GetById(int id)
        {
            var brand = _brandRepository.GetById(id);
            return brand == null ? null : new BrandDto
            {
                Id = brand.Id,
                Name = brand.Name
            };
        }

        public List<BrandDto> GetAll()
        {
            return _brandRepository.GetAll()
                .Select(b => new BrandDto
                {
                    Id = b.Id,
                    Name = b.Name
                }).ToList();
        }

        public int Create(CreateBrandDto dto)
        {
            var brand = new DataAccess.Entities.Brand()
            {
                Name = dto.Name,
                Description = dto.Description,
                Products = new List<Product>()
            };

            _brandRepository.Add(brand);
            return brand.Id;
        }

        public void Update(UpdateBrandDto dto)
        {
            var brand = _brandRepository.GetById(dto.Id);
            if (brand == null) throw new Exception("Brand not found");

            brand.Name = dto.Name;
            brand.Description = dto.Description;

            _brandRepository.Update(brand);
        }

        public void Delete(int id)
        {
            var brand = _brandRepository.GetById(id);
            if (brand == null) return;

            if (brand.Products?.Any() == true)
            {
                throw new InvalidOperationException("Cannot delete brand with associated products");
            }

            _brandRepository.Delete(id);
        }
    }
}
