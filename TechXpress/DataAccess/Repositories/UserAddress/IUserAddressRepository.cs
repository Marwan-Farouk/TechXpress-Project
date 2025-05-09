using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.USERADDRESS
{
    public interface IUserAddressRepository
    {
        Task<UserAddress?> GetById(int userId, int addressId);
        Task<List<UserAddress>> GetAll();
        Task Add(UserAddress userAddress);
        Task Update(UserAddress userAddress);
        Task Delete(int userId, int addressId);
        Task<List<UserAddress>> GetAddressesByUserId(int userId);
        Task<List<UserAddress>> SearchByCity(string city);
        Task<List<Order>?> GetOrdersByAddress(int userId, int addressId);
        Task<int> GetMaxId();
    }
}
