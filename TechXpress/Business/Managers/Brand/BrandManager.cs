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

        public async Task<BrandDto?> GetById(int id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            return brand == null ? null : new BrandDto
            {
                Id = brand.Id,
                Name = brand.Name,
                Description= brand.Description
            };
        }

        public async Task<List<BrandDto>> GetAll()
        {
            var brands = await _brandRepository.GetAllAsync();
            return brands.Select(b => new BrandDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description
                }).ToList();
        }

        public async Task<int> Create(CreateBrandDto dto)
        {
            var brand = new DataAccess.Entities.Brand()
            {
                Name = dto.Name,
                Description = dto.Description,
                Products = new List<Product>()
            };

            await _brandRepository.AddAsync(brand);
            return brand.Id;
        }

        public async Task Update(UpdateBrandDto dto)
        {
            var brand = await _brandRepository.GetByIdAsync(dto.Id);
            if (brand == null) throw new Exception("Brand not found");

            brand.Name = dto.Name;
            brand.Description = dto.Description;

             await _brandRepository.UpdateAsync(brand);
        }

        public async Task Delete(int id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand == null) return;

            if (brand.Products?.Any() == true)
            {
                throw new InvalidOperationException("Cannot delete brand with associated products");
            }

            await _brandRepository.DeleteAsync(id);
        }
    }
}
