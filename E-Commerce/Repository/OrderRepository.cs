using E_Commerce.IRepository;
using E_Commerce.Models;

namespace E_Commerce.Reposiory
{
    public class OrderRepository : IOrderRepository
    {
        public int CreateOrderFromCart(string userId, string street, string city, string paymentMethod)
        {
            throw new NotImplementedException();
        }

        public Order GetOrderById(int id)
        {
            throw new NotImplementedException();
        }

        public int GetOrderCountByDate(DateTime date)
        {
            throw new NotImplementedException();
        }

        public List<Order> GetOrdersByStatus(string status)
        {
            throw new NotImplementedException();
        }

        public List<Order> GetOrdersByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public decimal GetTotalRevenue(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrderStatus(int orderId, string status)
        {
            throw new NotImplementedException();
        }

        public void UpdatePaymentStatus(int orderId, string paymentStatus)
        {
            throw new NotImplementedException();
        }
    }
}
