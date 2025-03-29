using Business.DTOs.Orders;
using DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Managers.Orders
{
    public interface IOrderManager
    {
        // Order Creation
        Task CreateOrder(CreateOrderDto orderDto);

        // Order Retrieval
        Task<Order> GetOrderById(int id);
        Task<List<Order>> GetAllOrders();
        Task<List<Order>> GetOrdersByUserId(int userId);
        Task<List<Order>> GetOrdersByStatus(string status);
        Task<List<Order>> GetOrdersByUserIdAndStatus(int userId, string status);
        Task<List<Order>> GetOrdersInDateRange(DateTime startDate, DateTime endDate);

        // Order Management
        Task UpdateOrder(Order order);
        Task DeleteOrder(int id);

        // Additional methods from your OrderManager
        Task SaveChangesAsync();
    }
}