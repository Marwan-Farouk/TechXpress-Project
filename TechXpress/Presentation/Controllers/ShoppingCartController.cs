using Business.DTOs.Orders;
using Business.DTOs.Products;
using Business.Managers.Products;
using DataAccess.Entities;
using DataAccess.Repositories.ORDER;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using Business.Managers.Users;
using DataAccess.Repositories.USERADDRESS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Stripe.Checkout;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers
{
    public class ShoppingCartController : Controller
    {
        private const string CartCookieKey = "ShoppingCart";
        private readonly IOrderRepository _orderRepository;
        private readonly IProductManager _productManager;
        private readonly IUserManager _userManager;
        private readonly IAddressManager _addressManager;

        public ShoppingCartController(IOrderRepository orderRepository, IProductManager productManager,
            IUserManager userManager, IAddressManager addressManager)
        {
            _orderRepository = orderRepository;
            _productManager = productManager;
            _userManager = userManager;
            _addressManager = addressManager;
        }

        // ======== Manage Cart via Cookies ========
        private ShoppingCartViewModel GetCart()
        {
            var cartJson = Request.Cookies[CartCookieKey];
            return string.IsNullOrEmpty(cartJson)
                ? new ShoppingCartViewModel()
                : JsonConvert.DeserializeObject<ShoppingCartViewModel>(cartJson)!;
        }

        private void SaveCart(ShoppingCartViewModel cart)
        {
            var cartJson = JsonConvert.SerializeObject(cart);
            Response.Cookies.Append(CartCookieKey, cartJson, new CookieOptions { HttpOnly = true, Expires = DateTimeOffset.UtcNow.AddDays(7) });
        }

        private void ClearCart()
        {
            Response.Cookies.Delete(CartCookieKey);
        }

        // ======== Actions ========
        [Authorize(Roles = "Admin, Customer")]
        public IActionResult Index()
        {
            var cart = GetCart();
            return View("Index", cart);
        }

        // Add to Cart
        [HttpPost]
        [Authorize(Roles = "Admin, Customer")]
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
                existingItem.CalculateSubTotal();
            }
            else
            {
                var newItem = new CartItemViewModel
                {
                    ProductId = id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity,
                    ImageUrl = product.Image,
                };
                newItem.CalculateSubTotal();
                cart.Items.Add(newItem);

            }

            cart.TotalAmount = cart.Items.Sum(i => i.SubTotal);
            SaveCart(cart);

            return RedirectToAction("Index");
        }

        // Remove from Cart
        [HttpPost]
        [Authorize(Roles = "Admin, Customer")]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCart();

            var itemToRemove = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (itemToRemove != null)
            {
                cart.Items.Remove(itemToRemove);
                cart.TotalAmount = cart.Items.Sum(i => i.SubTotal);
                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }

        // Update Quantity
        [HttpPost]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            var cart = GetCart();
            var itemToUpdate = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (itemToUpdate != null)
            {
                var product = await _productManager.GetProductByIdAsync(productId);
                if (product == null || product.Stock < quantity)
                {
                    TempData["ErrorMessage"] = "Product not available or insufficient stock.";
                    return RedirectToAction("Index");
                }

                itemToUpdate.Quantity = quantity;
                itemToUpdate.CalculateSubTotal();
                cart.TotalAmount = cart.Items.Sum(i => i.SubTotal);
                SaveCart(cart);
            }
            else
            {
                TempData["Error"] = "Item not found in cart.";
            }
            return RedirectToAction("Index");
        }

        // Checkout
        [HttpGet]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> Checkout()
        {
            var cart = GetCart();
            if (!cart.Items.Any())
            {
                TempData["ErrorMessage"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }

            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity!.Name!);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found. Please log in again.";
                return RedirectToAction("Login", "Account");
            }

            var userAddresses = await _addressManager.GetAddressesByUserId(user.Id);
            var addressesVm = userAddresses.Select(address => new UserAddressViewModel
            {
                Id = address.Id,
                AddressLine = $"{address.Street} - {address.BuildingNumber} - {address.ApartmentNumber}",
                City = address.City,
                Country = address.Country
            }).ToList();

            if (!addressesVm.Any())
            {
                TempData["ErrorMessage"] = "Please add a shipping address before proceeding.";
                return RedirectToAction("AddAddress", "Account");
            }

            var checkoutVm = new CheckoutViewModel { Cart = cart, UserAddresses = addressesVm };
            return View(checkoutVm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Customer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(int addressId)
        {
            var cart = GetCart();
            if (!cart.Items.Any())
            {
                TempData["ErrorMessage"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }

            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity!.Name!);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found. Please log in again or contact support.";
                return RedirectToAction("Login", "Account");
            }

            var userAddresses = await _addressManager.GetAddressesByUserId(user.Id);
            if (userAddresses == null || !userAddresses.Any())
            {
                TempData["ErrorMessage"] = "Please add a shipping address before proceeding.";
                return RedirectToAction("Index");
            }

            var selectedAddress = userAddresses.FirstOrDefault(a => a.Id == addressId);
            if (selectedAddress == null)
            {
                TempData["ErrorMessage"] = $"Invalid address selected. AddressId: {addressId} does not exist for UserId: {user.Id}.";
                return RedirectToAction("Checkout", "ShoppingCart");
            }

            // Store products in dictionary to avoid tracking conflicts
            var productDict = new Dictionary<int, GetProductByIdDto>();

            // First check for sufficient stock for all items
            foreach (var item in cart.Items)
            {
                var product = await _productManager.GetProductByIdAsync(item.ProductId);
                if (product == null)
                {
                    TempData["ErrorMessage"] = $"Product with ID {item.ProductId} not found.";
                    return RedirectToAction("Index");
                }

                if (product.Stock < item.Quantity)
                {
                    TempData["ErrorMessage"] = $"Insufficient stock for product: {product.Name}.";
                    return RedirectToAction("Index");
                }

                // Store product for later use
                productDict[item.ProductId] = product;
            }

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

            var domain = Request.Scheme + "://" + Request.Host.Value + "/";
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                SuccessUrl = domain + "ShoppingCart/OrderConfirmed?sessionId={CHECKOUT_SESSION_ID}",
                CancelUrl = domain + "ShoppingCart/Checkout",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };

            foreach (var item in cart.Items)
            {
                var product = productDict[item.ProductId];
                var stripeLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.SubTotal * 100), // Convert to cents
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = product.Name,
                        },
                    },
                    Quantity = item.Quantity,
                };
                options.LineItems.Add(stripeLineItem);
            }

            var service = new Stripe.Checkout.SessionService();
            Session session = service.Create(options);
            order.StripeSessionId = session.Id;
            await _orderRepository.UpdateAsync(order);

            ClearCart();
            Response.Headers.Add("Location", session.Url);
            return Redirect(session.Url);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> OrderConfirmed()
        {
            var sessionId = Request.Query["sessionId"].ToString();
            if (string.IsNullOrEmpty(sessionId))
            {
                TempData["ErrorMessage"] = "Session ID is missing or invalid!";
                return RedirectToAction("Index");
            }

            var order = await _orderRepository.GetOrderByStripeSessionIdAsync(sessionId);
            if (order == null)
            {
                TempData["ErrorMessage"] = $"Order with Session ID {sessionId} not found.";
                return RedirectToAction("Index");
            }

            var service = new Stripe.Checkout.SessionService();
            var session = await service.GetAsync(order.StripeSessionId);
            if (session.PaymentStatus == "paid")
            {
                order.Status = "Paid";
                await _orderRepository.UpdateAsync(order);
            }

            return View("OrderConfirmation", order);
        }

        [HttpGet]
        public IActionResult GetCartCount()
        {
            var cart = GetCart();
            return Json(new { count = cart.Items.Sum(i => i.Quantity) });
        }




    }
}

