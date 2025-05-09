using Business.DTOs.Orders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Managers.Orders
{
    public interface IOrderManager
    {
        // Creation & Deletion
        Task CreateOrderAsync(OrderDto dto);
        Task DeleteOrderAsync(int id);

        // Retrieval
        Task<List<GetOrderDto>> GetAllOrdersAsync();
        Task<GetOrderDto> GetOrderByIdAsync(int id);
        Task<List<GetOrderDto>> GetOrdersByUserIdAsync(int userId);
        Task<List<GetOrderDto>> GetOrdersByStatusAsync(string status);
        

        // Management
        Task UpdateOrderAsync(OrderDto dto);
    }
}