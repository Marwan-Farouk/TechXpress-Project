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
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetById(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<Order> GetOrderWithDetails(int id)
        {
            return await _context.Orders
            .Include(o => o.User)
            .Include(o => o.Address)
            .Include(o => o.Payment)
            .Include(o => o.OrderItems)
            .ThenInclude(od => od.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Order>> GetAll()
        {
            return await _context.Orders.Include(o => o.User).Include(o => o.OrderItems).ToListAsync();
        }

        public async Task Add(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task Update(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order is not null)
            {
                _context.Orders.Remove(order);
            }
        }

        public async Task<List<Order>> GetOrdersByUserId(int userId)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Address)
                .Include(o => o.OrderItems)
                .ThenInclude(od => od.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Order>> GetOrdersByStatus(string status)
        {
            return await _context.Orders.Where(o => o.Status.ToLower() == status.ToLower()).ToListAsync();
        }

        public async Task<List<Order>> GetOrdersByUserIdAndStatus(int userId, string status)
        {
            return await _context.Orders.Where(o => o.UserId == userId && o.Status.ToLower() == status.ToLower()).ToListAsync();
        }

        public async Task<List<Order>> GetOrdersInDateRange(DateTime StartDate, DateTime EndDate)
        {
            return await _context.Orders
            .Where(o => o.OrderDate >= StartDate && o.OrderDate <= EndDate)
            .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
