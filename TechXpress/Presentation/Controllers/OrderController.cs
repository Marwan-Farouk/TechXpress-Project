using Business.DTOs.Orders;
using Business.Managers.Orders;
using Business.Managers.Products;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModel;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class OrderController : Controller
    {
        private readonly IProductManager _productManager;
        private readonly IOrderManager _orderManager;

        public OrderController(
            IProductManager productManager,
            IOrderManager orderManager)
        {
            _productManager = productManager;
            _orderManager = orderManager;
        }

        public IActionResult Cart()
        {
            var cartItems = _orderManager.GetCartItems();
            var viewModel = new CartViewModel
            {
                Items = cartItems.Select(item => new CartItemViewModel
                {
                    ProductId = item.Product.Id,
                    ProductName = item.Product.Name,
                    Price = item.Product.Price,
                    Quantity = item.Quantity,
                    Image = item.Product.Image
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            _orderManager.AddToCart(productId, quantity);
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            _orderManager.RemoveFromCart(productId);
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            _orderManager.UpdateCartItemQuantity(productId, quantity);
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            _orderManager.ClearCart();
            return RedirectToAction("Cart");
        }
    }
}