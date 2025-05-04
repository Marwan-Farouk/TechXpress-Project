using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Payments
{
    public class UpdatePaymentDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }
    }

}