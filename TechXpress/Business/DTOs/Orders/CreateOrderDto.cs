using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Orders
{
    public class CreateOrderDto
    {
        public int UserId { get; set; }
        public int AddressId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public List<int> ProductIds { get; set; }
    }

    public static class CreateOrderDtoExtentions
    {
        public static Order ToOrder(this CreateOrderDto dto)
            => new Order
            {
                UserId = dto.UserId,
                AddressId = dto.AddressId,
                TotalAmount = dto.TotalAmount,
                Status = dto.Status,
                PaymentId = default,
                OrderDate = DateTime.Now,
            };
    }
}
