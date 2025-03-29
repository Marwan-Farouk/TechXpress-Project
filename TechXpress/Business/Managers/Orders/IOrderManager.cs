using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Managers.Orders
{
    public interface IOrderManager
    {
        // Cart management
        void AddToCart(int productId, int quantity = 1);
        void RemoveFromCart(int productId);
        void UpdateCartItemQuantity(int productId, int quantity);
        void ClearCart();
        List<(Product Product, int Quantity)> GetCartItems();
        decimal GetCartTotal();
        int GetCartItemCount();
        bool IsCartEmpty();
    }
}
