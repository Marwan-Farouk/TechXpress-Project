namespace Business.DTOs.Orders
{
    public class OrderDto
    {
        public int Id { get; set; } // Added Id property
        public int UserId { get; set; }
        public int AddressId { get; set; }
        public List<CreateOrderDetailsDto> OrderItems { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending"; // Added Status property with default value
    }

    public class CreateOrderDetailsDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
    }

    public class GetOrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AddressId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public List<GetOrderDetailsDto> OrderItems { get; set; } = new();
    }

    public class GetOrderDetailsDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal => UnitPrice * Quantity;
    }
}