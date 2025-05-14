using DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories.ORDER
{
    public interface IOrderRepository
    {
        // CRUD Base
        Task<Order> GetByIdAsync(int id);
        Task<Order> GetByIdAsync(int id, bool includeDetails);
        Task<List<Order>> GetAllAsync();

        Task UpdateAsync(Order order);
        Task DeleteAsync(int id);
        Task<Order> AddAsync(Order order);
        // Advanced Queries
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        Task<List<Order>> GetOrdersByStatusAsync(string status);
        Task<List<Order>> GetOrdersInDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<Order>> GetOrdersByUserIdAndStatusAsync(int userId, string status); // Fixed name
        Task<Order?> GetOrderByStripeSessionIdAsync(string stripeSessionId);


        // Utility
        Task SaveChangesAsync();
    }
}