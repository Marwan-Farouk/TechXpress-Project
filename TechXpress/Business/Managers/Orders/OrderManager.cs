using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using DataAccess.Repositories.PRODUCT;

namespace Business.Managers.Orders
{
    public class OrderManager : IOrderManager
    {
        private readonly IProductRepository _productRepository;
        private Dictionary<int, int> _cartItems = new Dictionary<int, int>(); // ProductId, Quantity

        public OrderManager(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Cart management methods
        public void AddToCart(int productId, int quantity = 1)
        {
            if (_cartItems.ContainsKey(productId))
            {
                _cartItems[productId] += quantity;
            }
            else
            {
                _cartItems[productId] = quantity;
            }
        }

        public void RemoveFromCart(int productId)
        {
            if (_cartItems.ContainsKey(productId))
            {
                _cartItems.Remove(productId);
            }
        }

        public void UpdateCartItemQuantity(int productId, int quantity)
        {
            if (quantity <= 0)
            {
                RemoveFromCart(productId);
            }
            else if (_cartItems.ContainsKey(productId))
            {
                _cartItems[productId] = quantity;
            }
        }

        public void ClearCart()
        {
            _cartItems.Clear();
        }

        public List<(Product Product, int Quantity)> GetCartItems()
        {
            var result = new List<(Product, int)>();

            foreach (var item in _cartItems)
            {
                var product = _productRepository.GetById(item.Key);
                if (product != null)
                {
                    result.Add((product, item.Value));
                }
            }

            return result;
        }

        public decimal GetCartTotal()
        {
            decimal total = 0;
            foreach (var item in _cartItems)
            {
                var product = _productRepository.GetById(item.Key);
                if (product != null)
                {
                    total += product.Price * item.Value;
                }
            }
            return total;
        }

        public int GetCartItemCount()
        {
            return _cartItems.Sum(x => x.Value);
        }

        public bool IsCartEmpty()
        {
            return _cartItems.Count == 0;
        }
    }
}
