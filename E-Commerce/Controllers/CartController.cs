using E_Commerce.IRepository;
using E_Commerce.Models;
using E_Commerce.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    public class CartController(ICartRepository cartRepository) : Controller
    {
        public IActionResult Index()
        {
            Cart cart = cartRepository.GetCartByUserId(null);
            return View("Index",cart);
        }
       // [HttpPost]
        public IActionResult AddToCart(int productVariantId, int quantity = 1)
        {
            cartRepository.AddToCart(null, productVariantId, quantity);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            cartRepository.RemoveFromCart(cartItemId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int cartItemId, int quantity)
        {
            cartRepository.UpdateCartItemQuantity(cartItemId, quantity);
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public IActionResult MergeCart()
        {
            var sessionId = HttpContext.Session.GetString("CartSessionId");
            if (!string.IsNullOrEmpty(sessionId))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                cartRepository.MergeCarts($"SESSION_{sessionId}", userId);
            }
            return RedirectToAction("Index");
        }
    }
}
