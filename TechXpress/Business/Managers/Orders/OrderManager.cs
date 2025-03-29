using Business.DTOs.Orders;
using DataAccess.Entities;
using DataAccess.Repositories.ORDER;
using DataAccess.Repositories.PRODUCT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Managers.Orders
{
    public class OrderManager : IOrderManager
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrderManager(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task CreateOrder(CreateOrderDto orderDto)
        {
            // Your existing implementation
            var order = orderDto.ToOrder();
            await _orderRepository.Add(order);
            await _orderRepository.SaveChangesAsync();

            order.OrderItems = orderDto.ProductIds.Select(id => new OrderDetails
            {
                ProductId = id,
                OrderId = order.Id,
                Quantity = 1,
                UnitPrice = _productRepository.GetById(id).Price
            }).ToList();

            await _orderRepository.SaveChangesAsync();
        }

        public async Task<Order> GetOrderById(int id)
            => await _orderRepository.GetById(id);

        public async Task<List<Order>> GetAllOrders()
            => await _orderRepository.GetAll();

        public async Task<List<Order>> GetOrdersByUserId(int userId)
            => await _orderRepository.GetOrdersByUserId(userId);

        public async Task<List<Order>> GetOrdersByStatus(string status)
            => await _orderRepository.GetOrdersByStatus(status);

        public async Task<List<Order>> GetOrdersByUserIdAndStatus(int userId, string status)
            => await _orderRepository.GetOrdersByUserIdAndStatus(userId, status);

        public async Task<List<Order>> GetOrdersInDateRange(DateTime startDate, DateTime endDate)
            => await _orderRepository.GetOrdersInDateRange(startDate, endDate);

        public async Task UpdateOrder(Order order)
            => await _orderRepository.Update(order);

        public async Task DeleteOrder(int id)
            => await _orderRepository.Delete(id);

        public async Task SaveChangesAsync()
            => await _orderRepository.SaveChangesAsync();
    }
}