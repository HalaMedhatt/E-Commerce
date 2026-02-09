using E_Commerce.Models;
using E_Commerce.Models.Enum;
using E_Commerce.Repository;
using E_Commerce.ViewModels;

namespace E_Commerce.IRepository
{
    public interface IOrderRepository: IRepository<Order>
    {
        Task<int> CreateOrderFromCartAsync(string userId, CheckoutViewModel CheckoutVM);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<bool> CancelOrderAsync(int orderId, string userId);
        //List<Order> GetByUserId(string userId);
    }
}
