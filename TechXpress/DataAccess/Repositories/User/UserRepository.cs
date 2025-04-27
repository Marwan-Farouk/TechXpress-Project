using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.USER
{
    class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var userToDelete = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (userToDelete is not null)
            {
                _context.Users.Remove(userToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<User>> SearchByEmailAsync(string email)
        {
            return await _context.Users.Where(x => x.Email.Contains(email)).ToListAsync();
        }

        public async Task<List<User>> SearchByNameAsync(string name)
        {
            return await _context.Users.Where(x => (x.FirstName + " " + x.LastName).Contains(name)).ToListAsync();
        }

        public async Task<List<UserAddress>> GetUserAddressesAsync(int userId)
        {
            return await _context.UserAddresses.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<List<UserPhone>> GetUserPhonesAsync(int userId)
        {
            return await _context.UserPhones.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<List<Order>> GetUserOrdersAsync(int userId)
        {
            return await _context.Orders.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<List<Payment>> GetUserPaymentsAsync(int Id)
        {
            return await _context.Payments.Where(x => x.Id == Id).ToListAsync();
        }
    }
}
