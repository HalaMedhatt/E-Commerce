using E_Commerce.Models;
using E_Commerce.Models.Enum;
using E_Commerce.Repository;

namespace E_Commerce.IRepository
{
    public interface IOrderRepository: IRepository<Order>
    {
        int CreateOrderFromCart(string userId, int shippingAddressId);
        bool UpdateOrderStatus(int orderId, OrderStatus status);
        bool CancelOrder(int orderId, string userId);
        List<Order> GetByUserId(string userId);
    }
}
