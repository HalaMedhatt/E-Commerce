using E_Commerce.Models;
using E_Commerce.Models.Enum;
using E_Commerce.Repository;
using E_Commerce.ViewModels;

namespace E_Commerce.IRepository
{
    public interface IOrderRepository: IRepository<Order>
    {
        Task<int> CreateOrderFromCart(string userId, CheckoutViewModel CheckoutVM);
        bool UpdateOrderStatus(int orderId, OrderStatus status);
        bool CancelOrder(int orderId, string userId);
        List<Order> GetByUserId(string userId);
    }
}
