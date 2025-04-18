using Business.DTOs.Orders;

namespace Business.Managers.Orders
{
    public interface IOrderManager
    {
        // Order Creation
        Task CreateOrderAsync(OrderDto orderDto);

        // Order Retrieval
        Task<List<GetOrderDto>> GetAllOrdersAsync();
        Task<GetOrderDto> GetOrderByIdAsync(int id);
        Task<List<GetOrderDto>> GetOrdersByUserIdAsync(int userId);
        Task<List<GetOrderDto>> GetOrdersByStatusAsync(string status);

        // Order Management
        Task UpdateOrderAsync(OrderDto orderDto);
        Task DeleteOrderAsync(int id);
    }
}