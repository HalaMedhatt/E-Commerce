using E_Commerce.IRepository;
using E_Commerce.Models;
using E_Commerce.Models.Enum;
using E_Commerce.Repository;
using E_Commerce.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_Commerce.Reposiory
{
    public class OrderRepository(ECommerceDbContext context, ICartRepository cartRepository,IPaymobRepository paymobRepository) : IOrderRepository
    {
        public void Add(Order item)
        {
            context.Orders.Add(item);
        }
        public async Task<int> CreateOrderFromCart(string userId, CheckoutViewModel checkoutVM)
        {
            var cart = cartRepository.GetCartByUserId(userId);

            if (cart == null || !cart.CartItems.Any())
            {
                throw new InvalidOperationException("Cart is Empty!!!");
            }

            
            Order order = new Order
            {
                UserId = userId,
                ShippingAddressId = checkoutVM.ShippingAddressId,
                Status = OrderStatus.Pending,
                TotalCost = cartRepository.GetCartTotal(userId)
            };
            Add(order);
            Save();
            Payment payment = new Payment
            {
                PaymentMethod = checkoutVM.PaymentMethod,
                PaymentStatus = PaymentStatus.Pending,
                PaymentDate = DateTime.UtcNow,
                OrderId = order.Id

            };
            if(checkoutVM.PaymentMethod == PaymentMethod.Cash)
            {
                payment.TransactionRef = Guid.NewGuid().ToString();
            }
            context.Payments.Add(payment);
            Save();
			if (checkoutVM.PaymentMethod != PaymentMethod.Cash)
			{
				var paymentToken = await paymobRepository.GetPaymentTokenAsync(order, payment);
				payment.TransactionRef = paymentToken;
				context.Payments.Update(payment);
				Save();
			}
			foreach (var cartItem in cart.CartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductVariantId = cartItem.ProductVariantId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.ProductVariant.Price
                };
                context.OrderItems.Add(orderItem);
            }

            Save();
            cartRepository.ClearCart(userId);
            return order.Id;
        }

        public bool UpdateOrderStatus(int orderId, OrderStatus status)
        {
            var order = GetById(orderId);
            if (order == null)
            {
                return false;
            }

            order.Status = status;
            context.Orders.Update(order);
            Save();

            return true;
        }

        public bool CancelOrder(int orderId, string userId)
        {
            var order = GetById(orderId);
            if (order == null || order.UserId != userId)
            {
                return false;
            }

            if (order.Status == OrderStatus.Pending)
            {
                order.Status = OrderStatus.Cancelled;
                Edit(order);
                Save();
                return true;
            }

            return false;
        }
        public void Delete(int id)
        {
            var order = GetById(id);
            if (order != null)
            {
                context.Orders.Remove(order);
            }
        }

        public void Edit(Order item)
        {
            context.Orders.Update(item);
        }

        public List<Order> GetAll()
        {
            return context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .Include(o => o.Payment)
                .ToList();
        }

        public Order GetById(int id)
        {
            return context.Orders
               .Include(o => o.User)
               .Include(o => o.ShippingAddress)
               .Include(o => o.OrderItems)
                   .ThenInclude(oi => oi.ProductVariant)
                       .ThenInclude(pv => pv.Product)
               .Include(o => o.Payment)
               .FirstOrDefault(o => o.Id == id);
        }

        public List<Order> GetByUserId(string userId)
        {
           
            return context.Orders
               .Include(o => o.ShippingAddress)
               .Include(o => o.OrderItems)
                   .ThenInclude(oi => oi.ProductVariant)
                       .ThenInclude(pv => pv.Product)
               .Include(o => o.Payment)
               .Where(o => o.UserId == userId)
               .OrderByDescending(o => o.CreatedAt)
               .ToList();
        }

        public void Save()
        {
            context.SaveChanges();
        }

       
    }
}
