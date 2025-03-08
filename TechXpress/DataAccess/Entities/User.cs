using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime DateCreated { get; set; }
        public string Role { get; set; }
        public ICollection<UserPhone> userPhones { get; set; }
        public ICollection<UserAddress> userAddresses { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Payment>? Payments { get; set; } // Payments recived if user is seller 


    }
}
