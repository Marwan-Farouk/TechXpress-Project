using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.DTOs.Orders;
using Business.Managers.Products;
using DataAccess.Entities;
using DataAccess.Repositories.ORDER;
using DataAccess.Repositories.PRODUCT;

namespace Business.Managers.Orders
{
    public class OrderManager : IOrderManager
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        
        public OrderManager(IOrderRepository orderRepository , IProductRepository productRepository)
        {
            _orderRepository = orderRepository; 
            _productRepository = productRepository;
        }

        public async Task CreateOrder(CreateOrderDto orderDto)
        {
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



    }
}
