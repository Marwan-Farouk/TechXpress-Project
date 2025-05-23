﻿// OrderRepository.cs
using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Order> GetByIdAsync(int id)
        {
            return (await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Address)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id))!;
        }
        public async Task<Order> GetByIdAsync(int id, bool includeDetails = false)
        {
            if (includeDetails)
            {
                return await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .FirstOrDefaultAsync(o => o.Id == id);
            }
            return await _context.Orders.FindAsync(id);
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }

        public async Task<Order> AddAsync(Order order)
        {
            var result = await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return result.Entity;
        }


        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Order>> GetOrdersByStatusAsync(string status)
        {
            return await _context.Orders
                .Where(o => o.Status.ToLower() == status.ToLower())
                .ToListAsync();
        }

        public async Task<List<Order>> GetOrdersInDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetOrdersByUserIdAndStatusAsync(int userId, string status)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId && o.Status.ToLower() == status.ToLower())
                .ToListAsync();
        }
        public async Task<Order> GetOrderByStripeSessionIdAsync(string stripeSessionId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o=>o.Address)
                .FirstOrDefaultAsync(o => o.StripeSessionId == stripeSessionId);
        }



    }
}