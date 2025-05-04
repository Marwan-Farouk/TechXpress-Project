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
            var brand = _brandRepository.GetByIdAsync(id).Result; // Synchronous call for interface compliance
            return brand == null ? null : new BrandDto
            {
                Id = brand.Id,
                Name = brand.Name
            };
        }

        public List<BrandDto> GetAll()
        {
            var brands = _brandRepository.GetAllAsync().Result; // Synchronous call for interface compliance
            return brands.Select(b => new BrandDto
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

            _brandRepository.AddAsync(brand).Wait(); // Synchronous call for interface compliance
            return brand.Id;
        }

        public void Update(UpdateBrandDto dto)
        {
            var brand = _brandRepository.GetByIdAsync(dto.Id).Result; // Synchronous call for interface compliance
            if (brand == null) throw new Exception("Brand not found");

            brand.Name = dto.Name;
            brand.Description = dto.Description;

            _brandRepository.UpdateAsync(brand).Wait(); // Synchronous call for interface compliance
        }

        public void Delete(int id)
        {
            var brand = _brandRepository.GetByIdAsync(id).Result; // Synchronous call for interface compliance
            if (brand == null) return;

            if (brand.Products?.Any() == true)
            {
                throw new InvalidOperationException("Cannot delete brand with associated products");
            }

            _brandRepository.DeleteAsync(id).Wait(); // Synchronous call for interface compliance
        }

        public Task<BrandDto?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BrandDto>> GetAllAsync()
        {
            var brands = await _brandRepository.GetAllAsync();

            var brandDtos = brands.Select(b => new BrandDto
            {
                Id = b.Id,
                Name = b.Name
                
            }).ToList();

            return brandDtos;
        }

        public Task<int> CreateAsync(CreateBrandDto dto)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UpdateBrandDto dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}
