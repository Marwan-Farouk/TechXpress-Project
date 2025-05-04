using Business.DTOs.Orders;
using Business.DTOs.Products;
using Business.Managers.Products;
using DataAccess.Entities;
using DataAccess.Repositories.ORDER;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels;
using System.Text.Json;
using System.Threading.Tasks;
using DataAccess.Repositories.USERADDRESS;
using Microsoft.AspNetCore.Identity;

namespace Presentation.Controllers
{
    public class ShoppingCartController : Controller
    {
        private const string CartCookieKey = "ShoppingCart";
        private readonly IOrderRepository _orderRepository;
        private readonly IProductManager _productManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserAddressRepository _userAddressRepo;

        public ShoppingCartController(IOrderRepository orderRepository, IProductManager productManager,UserManager<User> userManager,IUserAddressRepository userAddressRepo)
        {
            _orderRepository = orderRepository;
            _productManager = productManager;
            _userManager = userManager;
            _userAddressRepo = userAddressRepo;
        }

        // ======== Manage Cart via Cookies ========
        private ShoppingCartViewModel GetCart()
        {
            var cartJson = Request.Cookies[CartCookieKey];
            return string.IsNullOrEmpty(cartJson)
                ? new ShoppingCartViewModel()
                : JsonSerializer.Deserialize<ShoppingCartViewModel>(cartJson)!;
        }

        private void SaveCart(ShoppingCartViewModel cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            Response.Cookies.Append(CartCookieKey, cartJson, new CookieOptions { HttpOnly = true, Expires = DateTimeOffset.UtcNow.AddDays(7) });
        }

        private void ClearCart()
        {
            Response.Cookies.Delete(CartCookieKey);
        }

        // ======== Actions ========
        public IActionResult Index()
        {
            var cart = GetCart();
            return View("Index", cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id, int quantity = 1)
        {
            var cart = GetCart();

            var product = await _productManager.GetProductByIdAsync(id);
            if (product == null || product.Stock < quantity)
            {
                return BadRequest("Product not available or insufficient stock.");
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItemViewModel
                {
                    ProductId = id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity
                });
            }

            cart.TotalAmount = cart.Items.Sum(i => i.SubTotal);
            SaveCart(cart);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var cart = GetCart();
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            var userAddresses = await _userAddressRepo.GetAddressesByUserId(user.Id);

            var addressesVm = userAddresses.Select(address => new UserAddressViewModel()
            {
                Id = address.AddressId,
                AddressLine = $"{address.Street} - {address.BuildingNumber} - {address.ApartmentNumber}",
                City = address.City,
                Country = address.Country
            }).ToList();
            
            var checkoutVm = new CheckoutViewModel { Cart = cart , UserAddresses = addressesVm };
            
            return View(checkoutVm);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(int addressId)
        {
            var cart = GetCart();

            // Store products in dictionary to avoid tracking conflicts
            var productDict = new Dictionary<int, GetProductByIdDto>();
            
            // First check for sufficient stock for all items
            foreach (var item in cart.Items)
            {
                var product = await _productManager.GetProductByIdAsync(item.ProductId);
                if (product == null)
                {
                    return BadRequest($"Product with ID {item.ProductId} not found");
                }
                
                if (product.Stock < item.Quantity)
                {
                    return BadRequest($"Insufficient stock for product: {product.Name}");
                }
                
                // Store product for later use
                productDict[item.ProductId] = product;
            }
        
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        
            var order = new Order
            {
                UserId = user.Id,
                AddressId = addressId,
                OrderDate = DateTime.UtcNow,
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
        
            // Deduct stock using the cached product data
            foreach (var item in cart.Items)
            {
                var product = productDict[item.ProductId];
                var updatedStock = product.Stock - item.Quantity;
                
                await _productManager.UpdateProductAsync(new UpdateProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = updatedStock,
                    BrandId = product.BrandId,
                    CategoryId = product.CategoryId,
                });
            }

            ClearCart();
            return RedirectToAction("OrderConfirmed", new { orderId = order.Id });
        }

        public async Task<IActionResult> OrderConfirmed(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            return View("OrderConfirmation",order);
        }

        [HttpGet]
        public IActionResult GetCartCount()
        {
            var cart = GetCart();
            return Json(new { count = cart.Items.Sum(i => i.Quantity) });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCart();

            // Find the item to remove
            var itemToRemove = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (itemToRemove != null)
            {
                cart.Items.Remove(itemToRemove);
                cart.TotalAmount = cart.Items.Sum(i => i.SubTotal); // Recalculate total amount
                SaveCart(cart); // Save updated cart
            }

            return RedirectToAction("Index");
        }
    }
}