using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProviderId { get; set; } // User Recieving the payment
        public User Provider { get; set; }
        public string Status { get; set; }
    }
}
