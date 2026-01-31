using E_Commerce.IRepository;
using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_Commerce.Repository
{
    public class CartRepository (ECommerceDbContext context,IHttpContextAccessor httpContextAccessor) : ICartRepository
    {
        public void Add(Cart item)
        {
            throw new NotImplementedException();
        }

        public bool AddToCart(string userId, int productVariantId, int quantity = 1)
        {
            userId ??= GetCartUserId();
            bool isSession = userId.StartsWith("SESSION_");

           
            Cart cart = GetCartByUserId(userId);

            if (cart == null)
            {
                cart = new Cart();
                if (isSession)
                {
                    cart.SessionId = userId;
                }
                else
                {
                    cart.UserId = userId;
                }
                context.Carts.Add(cart);
                Save();
            }

           
            CartItem? cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductVariantId == productVariantId);
            
            if (cartItem != null)
            {
                //if product already in cart, increase quantity
                cartItem.Quantity += quantity;
            }
            else
            {
                var productVariant = context.ProductVariants
                    .Include(pv => pv.Product) 
                    .FirstOrDefault(pv => pv.Id == productVariantId);

                if (productVariant == null)
                {
                   // Console.WriteLine($"ProductVariant with ID {productVariantId} not found");
                    return false;
                }

                if (productVariant.StockQuantity < quantity)
                {
                    quantity = productVariant.StockQuantity;
                    if (quantity <= 0)
                    {
                      //  Console.WriteLine($"ProductVariant {productVariantId} is out of stock");
                        return false;
                    }
                }
                cart.CartItems.Add(new CartItem
                {
                    ProductVariantId = productVariantId,
                    Quantity = quantity,
                });
            }
            Save();
            return true;
        }

        public void Edit(Cart item)
        {
            throw new NotImplementedException();
        }

        public List<Cart> GetAll()
        {
            return context.Carts
             .Include(c => c.CartItems)
                 .ThenInclude(ci => ci.ProductVariant)
                     .ThenInclude(pv => pv.Product).ToList();
        }

        public Cart GetById(int id)
        {
            throw new NotImplementedException();
        }

        private string GetCartUserId()
        {
            var user = httpContextAccessor.HttpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                return user.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            else
            {
                // SessionId for guest users
                var sessionId = httpContextAccessor.HttpContext.Session.GetString("CartSessionId");
                if (string.IsNullOrEmpty(sessionId))
                {
                    sessionId = Guid.NewGuid().ToString();
                    httpContextAccessor.HttpContext.Session.SetString("CartSessionId", sessionId);
                }
                return $"SESSION_{sessionId}";
            }
        }

        public Cart GetCartByUserId(string userId = null)
        {
            userId ??= GetCartUserId();


            List<Cart> carts = GetAll();
            Cart cart;
            if (userId.StartsWith("SESSION_"))
                cart = carts.FirstOrDefault(c => c.SessionId == userId);
            else 
                cart=carts.FirstOrDefault(c => c.UserId == userId);

            return cart;

        }
        public void RemoveFromCart(int cartItemId)
        {
            var cartItem = context.CartItems.Find(cartItemId);
            if (cartItem != null)
            {
                context.CartItems.Remove(cartItem);
                context.SaveChanges();
            }
        }

        public void UpdateCartItemQuantity(int cartItemId, int quantity)
        {
            var cartItem = context.CartItems.Find(cartItemId);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                context.SaveChanges();
            }
        }

        public void ClearCart(string userId)
        {
            var cart = GetCartByUserId(userId);
            if (cart != null)
            {
                context.CartItems.RemoveRange(cart.CartItems);
                context.SaveChanges();
            }
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCartItemCount(string userId)
        {
            var cart = GetCartByUserId(userId);
            return cart?.CartItems?.Sum(ci => ci.Quantity) ?? 0;
        }

        public decimal GetCartTotal(string userId)
        {
            var cart = GetCartByUserId(userId);

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
                return 0;

            return cart.CartItems.Sum(ci => ci.ProductVariant.Price * ci.Quantity);
        }
        public void MergeCarts(string sessionUserId, string authenticatedUserId)
        {
            var sessionCart = GetCartByUserId(sessionUserId);
            var userCart = GetCartByUserId(authenticatedUserId);

            if (sessionCart == null || !sessionCart.CartItems.Any())
                return;

            if (userCart == null)
            {
                sessionCart.UserId = authenticatedUserId;
                sessionCart.SessionId = null; 
                Save();
                return;
            }
            foreach (var sessionItem in sessionCart.CartItems)
            {
                // if same ProductVariantId found
                var existingItem = userCart.CartItems
                    .FirstOrDefault(ci => ci.ProductVariantId == sessionItem.ProductVariantId);

                if (existingItem != null)
                {
                    existingItem.Quantity += sessionItem.Quantity;
                }
                else
                {
                    userCart.CartItems.Add(new CartItem
                    {
                        ProductVariantId = sessionItem.ProductVariantId, 
                        Quantity = sessionItem.Quantity
                    });
                }
            }
            context.Carts.Remove(sessionCart);
            Save();
        }
        public void Save()
        {
            context.SaveChanges();
        }

        
    }
}
