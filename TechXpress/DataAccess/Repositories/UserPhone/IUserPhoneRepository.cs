using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.USERPHONE
{
    interface IUserPhoneRepository
    {
        UserPhone GetById(int userId, string phoneNumber);
        List<UserPhone> GetAll();
        void Add(UserPhone userPhone);
        void Update(UserPhone userPhone);
        void Delete(int userId, string phoneNumber);
        List<UserPhone> GetPhonesByUserId(int userId);
        List<UserPhone> SearchByPhoneNumber(string partialNumber);
    }
}
