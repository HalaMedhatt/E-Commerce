using E_Commerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    public class AddressController (ECommerceDbContext context, UserManager<ApplicationUser> userManager) : Controller
    {
        [HttpPost]
        public IActionResult SaveAdd(Address address)
        {

            
            var userId = GetCurrentUserId().Result;
            var address2 = new Address
            {
                City = address.City,
                Street = address.Street,
                UserId = userId

            };
            
            context.Addresses.Add(address2);
            context.SaveChanges();
            
            return RedirectToAction("Checkout","Order");
        }
        private async Task<string> GetCurrentUserId()
        {
            var user = await userManager.GetUserAsync(User);
            return user?.Id;
        }
    }
}
