using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.USER
{
    interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<List<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task<List<User>> SearchByEmailAsync(string email);
        Task<List<User>> SearchByNameAsync(string name);
        Task<List<UserAddress>> GetUserAddressesAsync(int userId);
        Task<List<UserPhone>> GetUserPhonesAsync(int userId);
        Task<List<Order>> GetUserOrdersAsync(int userId);
        Task<List<Payment>> GetUserPaymentsAsync(int Id);
    }
}
