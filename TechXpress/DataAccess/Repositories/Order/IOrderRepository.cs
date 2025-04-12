// IOrderRepository.cs
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories.ORDER
{
    public interface IOrderRepository
    {
        // الأساسية
        Task<Order> GetByIdAsync(int id);
        Task<List<Order>> GetAllAsync();
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(int id);

        // المتخصصة
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        Task<List<Order>> GetOrdersByStatusAsync(string status);
        Task<List<Order>> GetOrdersInDateRangeAsync(DateTime startDate, DateTime endDate);
        Task SaveChangesAsync();
    }
}