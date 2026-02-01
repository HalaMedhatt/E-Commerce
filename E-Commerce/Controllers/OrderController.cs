using E_Commerce.IRepository;
using E_Commerce.Models;
using E_Commerce.Models.Enum;
using E_Commerce.Reposiory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    public class OrderController(ECommerceDbContext context,IOrderRepository orderRepository, UserManager<ApplicationUser> userManager) : Controller
    {
        public IActionResult Index()
        {
            string userId = GetCurrentUserId().Result;
            var orders = orderRepository.GetByUserId(userId);
            return View(orders);
        }
        public IActionResult Details(int id)
        {
            var order = orderRepository.GetById(id);
            var userId = GetCurrentUserId().Result;

            if (order == null || order.UserId != userId)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost]
        public IActionResult CreateFromCart(int shippingAddressId)
        {
            try
            {
                var userId = GetCurrentUserId().Result;
                var orderId = orderRepository.CreateOrderFromCart(userId, shippingAddressId);

                return RedirectToAction("Details", new { id = orderId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Cart");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult UpdateStatus(int id, OrderStatus status)
        {
            var success = orderRepository.UpdateOrderStatus(id, status);

            if (!success)
            {
                return NotFound();
            }
            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        public IActionResult Cancel(int id)
        {
            var userId = GetCurrentUserId().Result;
            var success = orderRepository.CancelOrder(id, userId);

            if (!success)
            {
                return RedirectToAction("Details", new { id });
            }
            return RedirectToAction("Index");
        }
        private async Task<string> GetCurrentUserId()
        {
            var user = await userManager.GetUserAsync(User);
            return user?.Id;
        }
    }
}
