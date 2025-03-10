using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.ORDER
{
    interface IOrderRepository
    {
        Order GetById(int id);
        List<Order> GetAll();
        void Add(Order order);
        void Update(Order order);
        void Delete(int id);
        List<Order> GetOrdersByUserId(int userId);
        List<Order> GetOrdersByStatus(string status);
        List<Order> GetOrdersInDateRange(DateTime startDate, DateTime endDate);
        List<Order> GetOrdersByUserIdAndStatus(int userId, string status);
    }
}
