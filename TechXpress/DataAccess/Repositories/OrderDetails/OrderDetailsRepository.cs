using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.ORDERDETAILs
{
    class OrderDetailsRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public OrderDetails GetById(int id)
        {
            return _context.OrderDetails
            .Include(od => od.Order)
            .Include(od => od.Product)
            .FirstOrDefault(od => od.OrderId == id);
        }

        public List<OrderDetails> GetAll()
        {
            return _context.OrderDetails
            .Include(od => od.Order)
            .Include(od => od.Product)
            .ToList();
        }
        public void Add(OrderDetails orderDetails)
        {
            _context.OrderDetails.Add(orderDetails);
            _context.SaveChanges();
        }
        public void Update(OrderDetails orderDetails)
        {
            _context.OrderDetails.Update(orderDetails);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var orderDetails = _context.OrderDetails.FirstOrDefault(od => od.OrderId == id);
            if (orderDetails is not null)
            {

            _context.OrderDetails.Remove(orderDetails);
            _context.SaveChanges();
            }

        }
        public List<OrderDetails> GetByProductId(int ProductId)
        {
            return _context.OrderDetails
            .Include(od => od.Order)
            .Include(od => od.Product)
            .Where(od => od.ProductId == ProductId)
            .ToList();
        }
        public decimal GetTotalPrice(int orderId)
        {
            return _context.OrderDetails
            .Include(od => od.Order)
            .Include(od => od.Product)
            .Where(od => od.OrderId == orderId)
            .Sum(od => od.Product.Price * od.Quantity);
        }
    }
}
