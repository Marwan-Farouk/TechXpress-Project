using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.PAYMENT
{
    interface IPaymentRepository
    {
        Payment GetById(int id);
        List<Payment> GetAll();
        void Add(Payment payment);
        void Update(Payment payment);
        void Delete(int id);
        List<Payment> GetPaymentsByStatus(string status);
        List<Payment> GetPaymentsByProviderId(int providerId);
        List<Payment> GetPaymentsByOrderId(int orderId);
    }
}
