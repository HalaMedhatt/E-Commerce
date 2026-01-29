using E_Commerce.IRepository;
using E_Commerce.Models;

namespace E_Commerce.Repository
{
    public class CartRepository : ICartRepository
    {
        public void Add(Cart item)
        {
            throw new NotImplementedException();
        }

        public void AddToCart(string userId, int productId, int quantity)
        {
            throw new NotImplementedException();
        }

        public void ClearCart(string userId)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Edit(Cart item)
        {
            throw new NotImplementedException();
        }

        public List<Cart> GetAll()
        {
            throw new NotImplementedException();
        }

        public Cart GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Cart GetCartByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public int GetCartItemCount(string userId)
        {
            throw new NotImplementedException();
        }

        public decimal GetCartTotal(string userId)
        {
            throw new NotImplementedException();
        }

        public void MergeCarts(string sessionUserId, string authenticatedUserId)
        {
            throw new NotImplementedException();
        }

        public void RemoveFromCart(int cartItemId)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateCartItemQuantity(int cartItemId, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}
