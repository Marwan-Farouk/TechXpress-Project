// ShoppingCartController.cs
using DataAccess.Entities;
using DataAccess.Repositories.ORDER;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels;
using System.Text.Json;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class ShoppingCartController : Controller
    {
        private const string CartSessionKey = "ShoppingCart";
        private readonly IOrderRepository _orderRepository;

        public ShoppingCartController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // ======== إدارة السلة عبر Session ========
        private ShoppingCartViewModel GetCart()
        {
            var cartJson = HttpContext.Session.GetString(CartSessionKey);
            return string.IsNullOrEmpty(cartJson)
                ? new ShoppingCartViewModel()
                : JsonSerializer.Deserialize<ShoppingCartViewModel>(cartJson);
        }

        private void SaveCart(ShoppingCartViewModel cart)
        {
            HttpContext.Session.SetString(CartSessionKey, JsonSerializer.Serialize(cart));
        }

        private void ClearCart()
        {
            HttpContext.Session.Remove(CartSessionKey);
        }

        // ======== Actions ========
        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            var cart = GetCart();

            // (في الواقع: جلب بيانات المنتج من قاعدة البيانات)
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItemViewModel
                {
                    ProductId = productId,
                    ProductName = $"Product {productId}", // جلب الاسم الحقيقي من الـ Repository
                    Price = 10.99m, // جلب السعر الحقيقي من الـ Repository
                    Quantity = quantity
                });
            }

            cart.TotalAmount = cart.Items.Sum(i => i.SubTotal);
            SaveCart(cart);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(int addressId)
        {
            var cart = GetCart();

            var order = new Order
            {
                UserId = 1, // (يجب استبدالها بـ UserId الحقيقي)
                AddressId = addressId,
                OrderDate = DateTime.Now,
                TotalAmount = cart.TotalAmount,
                Status = "Pending",
                OrderItems = cart.Items.Select(i => new OrderDetails
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.Price,
                    Discount = 0
                }).ToList()
            };

            await _orderRepository.AddAsync(order);
            ClearCart();

            return RedirectToAction("OrderConfirmed", new { orderId = order.Id });
        }

        public async Task<IActionResult> OrderConfirmed(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            return View(order);
        }
        [HttpGet]
        public IActionResult GetCartCount()
        {
            var cart = GetCart();
            return Json(new { count = cart.Items.Sum(i => i.Quantity) });
        }
    }
}