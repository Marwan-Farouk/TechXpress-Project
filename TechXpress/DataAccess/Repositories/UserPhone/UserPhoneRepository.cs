using DataAccess.Contexts;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.USERPHONE
{
    class UserPhoneRepository
    {
        private readonly ApplicationDbContext _context;
        public UserPhoneRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public UserPhone GetById(int userId, string phoneNumber)
        {
            return _context.UserPhones.FirstOrDefault(up => up.UserId == userId && up.PhoneNumber == phoneNumber);
        }
        public List<UserPhone> GetAll()
        {
            return _context.UserPhones.ToList();
        }
        public void Add(UserPhone userPhone)
        {
            _context.UserPhones.Add(userPhone);
            _context.SaveChanges();
        }
        public void Update(UserPhone userPhone)
        {
            _context.UserPhones.Update(userPhone);
            _context.SaveChanges();
        }
        public void Delete(int userId, string phoneNumber)
        {
            var userPhone = _context.UserPhones.FirstOrDefault(up => up.UserId == userId && up.PhoneNumber == phoneNumber);
            if (userPhone != null)
            {
                _context.UserPhones.Remove(userPhone);
                _context.SaveChanges();
            }
        }
        public List<UserPhone> GetPhonesByUserId(int userId)
        {
            return _context.UserPhones.Where(up => up.UserId == userId).ToList();
        }
        public List<UserPhone> SearchByPhoneNumber(string partialNumber)
        {
            return _context.UserPhones.Where(up => up.PhoneNumber.Contains(partialNumber)).ToList();
        }
    }
}
