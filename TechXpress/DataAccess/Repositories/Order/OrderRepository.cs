using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.ORDER
{
    class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Order GetById(int id)
        {
            return _context.Orders
            .Include(o => o.User)
            .Include(o => o.Address)
            .Include(o => o.Payment)
            .Include(o => o.OrderItems)
            .ThenInclude(od => od.Product)
            .FirstOrDefault(o => o.Id == id);
        } 

        public List<Order> GetAll()
        {
            return _context.Orders.Include(o => o.User).Include(o=>o.OrderItems).ToList();
        }

        public void Add(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void Update(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var order = _context.Orders.Find(id);
            if(order is not null)
            {
            _context.Orders.Remove(order);
            _context.SaveChanges();
            }
        }

        public List<Order> GetOrdersByUserId(int userId) {
        return _context.Orders.Where(o=>o.UserId==userId).ToList();
        }

        public List<Order> GetOrdersByStatus(string status)
        {
            return _context.Orders.Where(o => o.Status.ToLower() == status.ToLower()).ToList();
        }

        public List<Order> GetOrdersByUserIdAndStatus(int userId, string status)
        {
            return _context.Orders.Where(o => o.UserId == userId && o.Status.ToLower() == status.ToLower()).ToList();
        }
        public List<Order> GetOrdersInDateRange(DateTime StartDate, DateTime EndDate)
        {
            return _context.Orders
            .Where(o => o.OrderDate >= StartDate && o.OrderDate <= EndDate)
            .ToList();
        }



    }
}
