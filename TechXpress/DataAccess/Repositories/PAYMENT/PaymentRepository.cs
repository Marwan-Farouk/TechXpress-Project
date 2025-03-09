using DataAccess.Contexts;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.PAYMENT
{
    class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;
        
        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Payment GetById(int id)
        {
            return _context.Payments.FirstOrDefault(p => p.Id == id);
        }
        public List<Payment> GetAll()
        {
            return _context.Payments.ToList(); 
        }
        public void Add(Payment payment)
        {
            _context.Payments.Add(payment);
            _context.SaveChanges();
        }
        public void Update(Payment payment)
        {
            _context.Payments.Update(payment);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var payment = _context.Payments.FirstOrDefault(p => p.Id == id);
            if (payment is not null)
            {
            _context.Payments.Remove(payment);
            _context.SaveChanges();

            }
        }
        public List<Payment> GetPaymentsByStatus(string status)
        {
            return _context.Payments.Where(p => p.Status== status).ToList();
        }
        public List<Payment> GetPaymentsByOrderId(int orderId)
        {
            return _context.Payments.Where(p => p.OrderId == orderId).ToList();
        }
        public List<Payment> GetPaymentsByProviderId(int providerID)
        {
            return _context.Payments.Where(p => p.ProviderId == providerID).ToList();
        }
    }
}
