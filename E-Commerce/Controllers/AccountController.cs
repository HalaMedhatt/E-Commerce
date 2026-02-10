using System.Security.Claims;
using E_Commerce.IRepository;
using E_Commerce.Models;
using E_Commerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly ICartRepository cartRepository;
	public AccountController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager,ICartRepository _cartRepository)
    {
        userManager = _userManager;
        signInManager = _signInManager;
        cartRepository= _cartRepository;
    }

    #region Register

    [HttpGet]
    public IActionResult Register()
    {
        return View("Register");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            ApplicationUser newUser = new ApplicationUser()
            {
                UserName = model.UserName,
                FirstName =  model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PasswordHash = model.Password,
                PhoneNumber = model.PhoneNumber,
                Avatar = "default.jpg" 
            };

            IdentityResult result = await userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, "User");
                
                await signInManager.SignInWithClaimsAsync(newUser, false, 
                    new[] { new Claim("Avatar", newUser.Avatar ?? "default.jpg") });

				var sessionId = HttpContext.Session.GetString("CartSessionId");
				if (!string.IsNullOrEmpty(sessionId))
				{
					var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
					cartRepository.MergeCarts($"SESSION_{sessionId}", userId);
				}
				return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        
        
        return View("Register", model);
    }

    #endregion

    
    #region LogIn

    [HttpGet]
    public IActionResult LogIn()
    {
        
        return View("LogIn");
    }
    
    
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogIn(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            ApplicationUser user = await userManager.FindByEmailAsync(model.Email);
        
            if (user != null)
            {
                if (user.IsDeleted && model.ReactivateAccount)
                {
                    user.IsDeleted = false;
                    user.DeletedAt = null; 
                    await userManager.UpdateAsync(user);
                    
                }

                bool correct = await userManager.CheckPasswordAsync(user, model.Password);

                if (correct && !user.IsDeleted)
                {
                    await signInManager.SignInWithClaimsAsync(user, model.RememberMe, 
                    new List<Claim> { new Claim("Avatar", user.Avatar ?? "default.jpg") });
					var sessionId = HttpContext.Session.GetString("CartSessionId");
					if (!string.IsNullOrEmpty(sessionId))
					{
						var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
						cartRepository.MergeCarts($"SESSION_{sessionId}", userId);
					}
					return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Invalid Account or Account Deleted");
        }

        return View("LogIn", model);
    }


    #endregion

    
    #region LogOut

    public async Task<IActionResult> LogOut()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("LogIn", "Account");
    }

    #endregion
}