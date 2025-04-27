using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.Entities
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<UserAddress> userAddresses { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Payment>? Payments { get; set; } // Payments recived if user is seller


    }
}
