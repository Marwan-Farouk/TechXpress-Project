using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.ORDERDETAILs
{
    interface IOrderDetailsRepository
    {
        OrderDetails GetById(int orderId);
        List<OrderDetails> GetAll();
        void Add(OrderDetails orderDetails);
        void Update(OrderDetails orderDetails);
        void Delete(int orderId);
        List<OrderDetails> GetByProductId(int productId);
        decimal CalculateTotal(int orderId);
    }
}
