using Business.DTOs.Orders;
using DataAccess.Entities;
using DataAccess.Repositories.ORDER;

namespace Business.Managers.Orders
{
    public class OrderManager : IOrderManager
    {
        private readonly IOrderRepository _orderRepository;

        public OrderManager(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // Create a new order
        public async Task CreateOrderAsync(OrderDto dto)
        {
            var order = new Order
            {
                UserId = dto.UserId,
                AddressId = dto.AddressId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = dto.TotalAmount,
                Status = "Pending",
                OrderItems = dto.OrderItems.Select(item => new OrderDetails
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Discount = item.Discount
                }).ToList()
            };

            await _orderRepository.AddAsync(order);
        }

        // Retrieve all orders
        public async Task<List<GetOrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.Select(order => new GetOrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                AddressId = order.AddressId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                OrderItems = order.OrderItems.Select(item => new GetOrderDetailsDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            }).ToList();
        }

        // Retrieve an order by ID
        public async Task<GetOrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) throw new KeyNotFoundException("Order not found.");

            return new GetOrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                AddressId = order.AddressId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                OrderItems = order.OrderItems.Select(item => new GetOrderDetailsDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            };
        }

        // Retrieve orders by user ID
        public async Task<List<GetOrderDto>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return orders.Select(order => new GetOrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                AddressId = order.AddressId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                OrderItems = order.OrderItems.Select(item => new GetOrderDetailsDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            }).ToList();
        }

        // Retrieve orders by status
        public async Task<List<GetOrderDto>> GetOrdersByStatusAsync(string status)
        {
            var orders = await _orderRepository.GetOrdersByStatusAsync(status);
            return orders.Select(order => new GetOrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                AddressId = order.AddressId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                OrderItems = order.OrderItems.Select(item => new GetOrderDetailsDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            }).ToList();
        }

        // Update an order
        public async Task UpdateOrderAsync(OrderDto dto)
        {
            var order = await _orderRepository.GetByIdAsync(dto.Id);
            if (order == null) throw new KeyNotFoundException("Order not found.");

            order.AddressId = dto.AddressId;
            order.TotalAmount = dto.TotalAmount;
            order.Status = dto.Status;
            order.OrderItems = dto.OrderItems.Select(item => new OrderDetails
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                Discount = item.Discount
            }).ToList();

            await _orderRepository.UpdateAsync(order);
        }

        // Delete an order
        public async Task DeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) throw new KeyNotFoundException("Order not found.");

            await _orderRepository.DeleteAsync(id);
        }
    }
}