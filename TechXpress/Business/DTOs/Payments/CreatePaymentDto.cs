using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Payments
{
    public class CreatePaymentDto
    {
        public decimal Amount { get; set; }
        public string Method { get; set; }
    }

}
