using E_Commerce.IRepository;
using E_Commerce.Models;
using E_Commerce.Models.Enum;
using E_Commerce.Reposiory;
using E_Commerce.Repository;
using E_Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    public class OrderController(ECommerceDbContext context,
        IOrderRepository orderRepository,
        UserManager<ApplicationUser> userManager,
		IPaymobRepository paymobRepository,
		IConfiguration configuration) : Controller
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
        public async Task<IActionResult> Checkout(CheckoutViewModel checkoutVM)
        {
            try
            {
                var userId = await GetCurrentUserId();
                var orderId = await orderRepository.CreateOrderFromCart(userId, checkoutVM);
				// إذا كانت طريقة الدفع غير نقدي، نعيد توجيه المستخدم إلى صفحة دفع Paymob
				if (checkoutVM.PaymentMethod != PaymentMethod.Cash)
				{
					// الحصول على الطلب لاستخراج رمز الدفع
					var order = orderRepository.GetById(orderId);
					var paymentToken = order.Payment.TransactionRef;

					// بناء رابط دفع Paymob
					var iframeId = configuration["Paymob:IframeId"];
					var paymobUrl = $"https://accept.paymob.com/api/acceptance/iframes/{iframeId}?payment_token={paymentToken}";

					return Redirect(paymobUrl);
				}
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
