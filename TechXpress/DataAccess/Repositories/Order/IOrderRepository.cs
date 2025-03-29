using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.ORDER
{
    public interface IOrderRepository
    {
        Task<Order> GetById(int id);
        Task<List<Order>> GetAll();
        Task<Order> GetOrderWithDetails(int id);
        Task Add(Order order);
        Task Update(Order order);
        Task Delete(int id);
        Task<List<Order>> GetOrdersByUserId(int userId);
        Task<List<Order>> GetOrdersByStatus(string status);
        Task<List<Order>> GetOrdersInDateRange(DateTime startDate, DateTime endDate);
        Task<List<Order>> GetOrdersByUserIdAndStatus(int userId, string status);
        Task SaveChangesAsync();
    }
}
