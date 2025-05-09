using Business.Managers.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels;

namespace Presentation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderManager _orderManager;

        public OrderController(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        // Action to display all orders
        public async Task<IActionResult> Index()
        {
            var orders = await _orderManager.GetAllOrdersAsync();

            // Map orders to OrderViewModel
            var orderViewModels = orders.Select(order => new OrderViewModel
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                OrderItems = order.OrderItems.Select(item => new OrderDetailsViewModel
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            }).ToList();

            return View(orderViewModels);
        }
        [HttpGet]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> OrderDetails(int id)
        {
            var order = await _orderManager.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var orderViewModel = new OrderViewModel
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                OrderItems = order.OrderItems.Select(item => new OrderDetailsViewModel
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            };

            return View(orderViewModel);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminOrders()
        {
            var orders = await _orderManager.GetAllOrdersAsync();
            return View(orders);
        }

    }
}