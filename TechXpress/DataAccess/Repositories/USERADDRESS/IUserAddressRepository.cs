using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.USERADDRESS
{
    interface IUserAddressRepository
    {
        UserAddress GetById(int userId, int addressId);
        List<UserAddress> GetAll();
        void Add(UserAddress userAddress);
        void Update(UserAddress userAddress);
        void Delete(int userId, int addressId);
        List<UserAddress> GetAddressesByUserId(int userId);
        List<UserAddress> SearchByCity(string city);
        List<Order> GetOrdersByAddress(int userId, int addressId);
    }
}
