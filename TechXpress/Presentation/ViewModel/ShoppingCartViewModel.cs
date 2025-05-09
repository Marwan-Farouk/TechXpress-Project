using System.Collections.Generic;
using Business.DTOs.Orders;

namespace Presentation.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();
        public decimal TotalAmount { get; set; }
        public int UserId { get; set; }
        public int AddressId { get; set; }
    }

    public class CartItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
        public string ImageUrl { get; set; }

    public void CalculateSubTotal()
        {
            SubTotal = Price * Quantity;
        }
    }

    public class CheckoutViewModel
    {
        public ShoppingCartViewModel Cart { get; set; }
        public List<UserAddressViewModel> UserAddresses { get; set; }
    }
}

public class UserAddressViewModel
{
    public int Id { get; set; }
    public string AddressLine { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
}