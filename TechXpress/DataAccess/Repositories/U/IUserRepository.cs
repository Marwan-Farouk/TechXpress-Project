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
        User GetById(int id);
        List<User> GetAll();
        void Add(User user);
        void Update(User user);
        void Delete(int id);
        List<User> SearchByEmail(string email);
        List<User> SearchByName(string name);
        List<UserAddress> GetUserAddresses(int userId);
        List<UserPhone> GetUserPhones(int userId);
        List<Order> GetUserOrders(int userId);
        List<Payment> GetUserPayments(int Id);
    }
}
