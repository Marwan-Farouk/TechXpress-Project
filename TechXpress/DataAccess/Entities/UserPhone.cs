using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class UserPhone
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public string PhoneNumber { get; set; }
    }
}
