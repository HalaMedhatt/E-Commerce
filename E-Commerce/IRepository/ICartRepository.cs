using E_Commerce.Models;
using E_Commerce.Repository;

namespace E_Commerce.IRepository
{
    public interface ICartRepository: IRepository<Cart>
    {
        Cart GetCartByUserId(string userId);
        bool AddToCart(string userId, int productId, int quantity);
        void RemoveFromCart(int cartItemId);
        void UpdateCartItemQuantity(int cartItemId, int quantity);
        void ClearCart(string userId);
        int GetCartItemCount(string userId);
        decimal GetCartTotal(string userId);

        // for gust users after login
        void MergeCarts(string sessionUserId, string authenticatedUserId);

    }
}
