using DataAccess.Contexts;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.USER
{
    class UserRepository:IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public User? GetById(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }
        public List<User> GetAll() 
        {
            return _context.Users.ToList();
        }
        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }   
        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var userToDelete = _context.Users.FirstOrDefault(x => x.Id == id);
            if(userToDelete is not null)
            {

            _context.Users.Remove(userToDelete);
            _context.SaveChanges();
            }
        }
        public List<User> SearchByEmail(string email)
        {
            return _context.Users.Where(x => x.Email.Contains(email)).ToList();
        }

        public List<User> SearchByName(string name)
        {
            return _context.Users.Where(x => (x.FirstName+" "+x.LastName).Contains(name)).ToList();
        }

        public List<UserAddress> GetUserAddresses(int userId)
        {
            return _context.UserAddresses.Where(x => x.UserId == userId).ToList();

        }

        public List<UserPhone> GetUserPhones(int userId)
        {
            return _context.UserPhones.Where(x => x.UserId == userId).ToList();
        }

        public List<Order> GetUserOrders(int userId) => _context.Orders.Where(x => x.UserId == userId).ToList();
        public List<Payment> GetUserPayments(int Id) => _context.Payments.Where(x => x.Id == Id).ToList();
    }
}
