using E_Commerce.Models;

namespace E_Commerce.IRepository
{
    public interface IOrderRepository
    {
        Order GetOrderById(int id);
        List<Order> GetOrdersByUserId(string userId);
        List<Order> GetOrdersByStatus(string status);
        int CreateOrderFromCart(string userId, string street, string city, string paymentMethod);
        void UpdateOrderStatus(int orderId, string status);
        void UpdatePaymentStatus(int orderId, string paymentStatus);
        decimal GetTotalRevenue(DateTime startDate, DateTime endDate);
        int GetOrderCountByDate(DateTime date);
    }
}
