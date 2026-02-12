using System.Security.Claims;
using E_Commerce.Models;
using E_Commerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace E_Commerce.Controllers;

public class ProfileController : Controller
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;

    public ProfileController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager)
    {
        userManager = _userManager;
        signInManager = _signInManager;
    }

    #region GetProfile

    public async Task<IActionResult> GetProfile()
    {
        // ApplicationUser user = await userManager.GetUserAsync(User);
    
        ApplicationUser user = await userManager.Users
            .FirstOrDefaultAsync(u => u.Id == userManager.GetUserId(User));
    
        if (user == null)
            return RedirectToAction("Login", "Account");
        
        ProfileViewModel uservm = new ProfileViewModel
        {
            FirstName =  user.FirstName,
            LastName =  user.LastName,
            UserName =  user.UserName,
            Email = user.Email,
            Phone = user.PhoneNumber,
            Avatar    = user.Avatar
            
        };
    
        return View("GetProfile", uservm);
    }


    #endregion


    #region Edit Profile Get

    [HttpGet]
    public async Task<IActionResult> EditProfile()
    {
        // ApplicationUser user = await userManager.GetUserAsync(User);
        
        ApplicationUser user = await userManager.Users
            .FirstOrDefaultAsync(u => u.Id == userManager.GetUserId(User));
        
        if (user == null)
            return RedirectToAction("Login", "Account");

        var avatarPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Avatar/");
        ViewBag.Avatars = Directory.GetFiles(avatarPath)
            .Select(Path.GetFileName)
            .ToList();

        ProfileViewModel uservm = new ProfileViewModel
        {
            FirstName = user.FirstName,
            LastName  = user.LastName,
            UserName  = user.UserName,
            Email     = user.Email,
            Phone     = user.PhoneNumber,
            Avatar    = user.Avatar
        };

        return View("EditProfile", uservm);
    }



    #endregion

    #region Edit Profile Post

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProfile(ProfileViewModel uservm)
    {
        
        ModelState.Remove("Password");
        ModelState.Remove("ConfirmPassword");
        ModelState.Remove("CurrentPassword");
        ModelState.Remove("Avatars");


        var avatarPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Avatar/");
        ViewBag.Avatars = Directory.GetFiles(avatarPath)
            .Select(Path.GetFileName)
            .ToList();

        
        if (!string.IsNullOrEmpty(uservm.Avatar) && !ViewBag.Avatars.Contains(uservm.Avatar))
        {
            ModelState.AddModelError("Avatar", "Invalid avatar selected.");
        }

        if (!ModelState.IsValid)
        {
            return View("EditProfile", uservm);
        }

        // ApplicationUser user = await userManager.GetUserAsync(User);
        
        ApplicationUser user = await userManager.Users
            .FirstOrDefaultAsync(u => u.Id == userManager.GetUserId(User));
        
        if (user == null)
            return RedirectToAction("Login", "Account");

        user.FirstName = uservm.FirstName;
        user.LastName  = uservm.LastName;
        user.UserName  = uservm.UserName;
        user.Email     = uservm.Email;
        user.PhoneNumber = uservm.Phone;
        user.Avatar    = uservm.Avatar; 

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View("EditProfile", uservm);
        }
        
        await signInManager.SignOutAsync();

        var claims = new List<Claim>
        {
            new Claim("Avatar", user.Avatar ?? "default.jpg"),
            new Claim("FullName", $"{user.FirstName} {user.LastName}")
        };

        await signInManager.SignInWithClaimsAsync(user, isPersistent: false, claims);

        return RedirectToAction("GetProfile");

    }


    #endregion

    #region Change Password Get

    [HttpGet]
    public async Task<IActionResult> ChangePassword()
    {
        ApplicationUser user = await userManager.GetUserAsync(User);

        if (user == null)
            return RedirectToAction("Login", "Account");
        ProfileViewModel uservm = new ProfileViewModel();
        
        return  View("ChangePassword", uservm);
    }

    #endregion

    #region Change Password Post

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ProfileViewModel uservm)
    {
        ModelState.Remove("Avatars");
        ModelState.Remove("Email");
        ModelState.Remove("LastName");
        ModelState.Remove("UserName");
        ModelState.Remove("FirstName");
        if (!ModelState.IsValid)
        {
            return View("EditProfile", uservm);
        }
        ApplicationUser user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }
        
        var result = await userManager.ChangePasswordAsync(user, uservm.CurrentPassword , uservm.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View("ChangePassword", uservm);
        }

        return RedirectToAction("GetProfile");
    }


    #endregion

    #region Delete Account
    
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAccount()
    {
        ApplicationUser user = await userManager.GetUserAsync(User);

        if (user == null)
            return RedirectToAction("Login", "Account");

        user.IsDeleted = true;
        user.DeletedAt = DateTime.UtcNow;

        await userManager.UpdateAsync(user);

        await signInManager.SignOutAsync();

        return RedirectToAction("Login", "Account");
    }

    

    #endregion

    
   
}