using DataAccess.Contexts;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.USERADDRESS
{
    class UserAddressRepository: IUserAddressRepository
    {
        private readonly ApplicationDbContext _context;
        public UserAddressRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public UserAddress? GetById(int userId, int addressId)
        {
            return _context.UserAddresses.FirstOrDefault(x => x.UserId == userId && x.AddressId == addressId);
        }
        public List<UserAddress> GetAll() { return _context.UserAddresses.ToList(); }
        public void Add(UserAddress userAddress)
        {
            _context.UserAddresses.Add(userAddress);
            _context.SaveChanges();
        }
        public void Update(UserAddress userAddress)

        {
_context.UserAddresses.Update(userAddress);
            _context.SaveChanges();
        }
            public void Delete(int userId, int addressId)
        {
            var userAddress = _context.UserAddresses.FirstOrDefault(x => x.UserId == userId && x.AddressId == addressId);
            if (userAddress is not null)
            {

            _context.UserAddresses.Remove(userAddress);
            _context.SaveChanges();
        }
            } 
        public List<UserAddress> GetAddressesByUserId(int userId)
        {
            return _context.UserAddresses.Where(x => x.UserId == userId).ToList();
        }
        public List<UserAddress> SearchByCity(string city)
        {
            return _context.UserAddresses.Where(x => x.City == city).ToList();
        }
        public List<Order> GetOrdersByAddress(int userId, int addressId)
        {
            return _context.Orders.Where(x => x.UserId == userId && x.AddressId == addressId).ToList();
        }
    }
}
