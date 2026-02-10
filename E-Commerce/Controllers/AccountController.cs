using System.Security.Claims;
using E_Commerce.Models;
using E_Commerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;

    public AccountController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager)
    {
        userManager = _userManager;
        signInManager = _signInManager;
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
                bool correct = await userManager.CheckPasswordAsync(user, model.Password);
                if (correct)
                {
                    
                    await signInManager.SignInWithClaimsAsync(user, model.RememberMe, 
                        new List<Claim> { new Claim("Avatar", user.Avatar ?? "default.jpg") });
                    
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Invalid Account");
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