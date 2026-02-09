using E_Commerce.Models;
using E_Commerce.Repository;

namespace E_Commerce.IRepository
{
    public interface ICartRepository: IRepository<Cart>
    {
        Task<Cart> GetCartByUserIdAsync(string userId);
        Task<bool> AddToCartAsync(string userId, int productId, int quantity);
        void RemoveFromCart(int cartItemId);
        void UpdateCartItemQuantity(int cartItemId, int quantity);
        void ClearCart(string userId);
        Task<int> GetCartItemCountAsync(string userId);
        Task<decimal> GetCartTotalAsync(string userId);

        // for gust users after login
        void MergeCarts(string sessionUserId, string authenticatedUserId);

    }
}
