using System.Collections.Generic;
using System.Linq;

namespace Presentation.ViewModel
{
    public class CartItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total => Price * Quantity;
        public string Image { get; set; }
    }

    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();
        public decimal Total => Items.Sum(item => item.Total);
        public int ItemCount => Items.Sum(item => item.Quantity);
    }
}
