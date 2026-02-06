using E_Commerce.IRepository;
using E_Commerce.Models;
using E_Commerce.Models.Enum;
using E_Commerce.Reposiory;
using E_Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    public class OrderController(ECommerceDbContext context,IOrderRepository orderRepository, UserManager<ApplicationUser> userManager) : Controller
    {
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var orders = orderRepository.GetAll();
            return View(orders);
        }
        public IActionResult Details(int id)
        {
            var order = orderRepository.GetById(id);
            var userId = GetCurrentUserId().Result;

            if (order == null || (order.UserId != userId && !User.IsInRole("Admin")))
            {
                return NotFound();
            }
            return View(order);
        }
        [HttpGet]
        public IActionResult Checkout()
        {
            var userId = GetCurrentUserId().Result;
            var addresses = context.Addresses
                .Where(a => a.UserId == userId)
                .ToList();

            var checkoutVM = new CheckoutViewModel
            {
                Addresses = addresses
            };

            return View("Checkout", checkoutVM);

        }

        [HttpPost]
        public IActionResult Checkout(CheckoutViewModel checkoutVM)
        {
            try
            {
                var userId = GetCurrentUserId().Result;
                var orderId = orderRepository.CreateOrderFromCart(userId, checkoutVM);

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
