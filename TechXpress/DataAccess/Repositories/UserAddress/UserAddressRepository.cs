using DataAccess.Contexts;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.USERADDRESS
{
    public class UserAddressRepository: IUserAddressRepository
    {
        private readonly ApplicationDbContext _context;
        public UserAddressRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<UserAddress?> GetById(int userId, int addressId)
        {
            return await _context.UserAddresses.FirstOrDefaultAsync(x => x.UserId == userId && x.AddressId == addressId);
        }

        public async Task<List<UserAddress>> GetAll()
        {
            return await _context.UserAddresses.ToListAsync();
        }
        public async Task Add(UserAddress userAddress)
        {
            await _context.UserAddresses.AddAsync(userAddress);
            await _context.SaveChangesAsync();
        }
        public async Task Update(UserAddress userAddress)

        {
            _context.UserAddresses.Update(userAddress);
            await _context.SaveChangesAsync();
        }
            public async Task Delete(int userId, int addressId)
        {
            var userAddress = await _context.UserAddresses.FirstOrDefaultAsync(x => x.UserId == userId && x.AddressId == addressId);
            if (userAddress is not null)
            {
                _context.UserAddresses.Remove(userAddress);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<UserAddress>> GetAddressesByUserId(int userId)
        {
            return await _context.UserAddresses.Where(x => x.UserId == userId).ToListAsync();
        }
        public async Task<List<UserAddress>> SearchByCity(string city)
        {
            return await _context.UserAddresses.Where(x => x.City == city).ToListAsync();
        }
        public async Task<List<Order>?> GetOrdersByAddress(int userId, int addressId)
        {
            return await _context.Orders.Where(x => x.UserId == userId && x.AddressId == addressId).ToListAsync();
        }
        public async Task<int> GetMaxId()
        {
            if (!await _context.UserAddresses.AnyAsync()) return 0;
            return await _context.UserAddresses.MaxAsync(p => p.AddressId);
        }
    }
}
