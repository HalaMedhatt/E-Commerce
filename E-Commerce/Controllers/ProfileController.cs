using E_Commerce.Models;
using E_Commerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        ApplicationUser user = await userManager.GetUserAsync(User);

        if (user == null)
            return RedirectToAction("Login", "Account");
        
        ProfileViewModel uservm = new ProfileViewModel
        {
            FirstName =  user.FirstName,
            LastName =  user.LastName,
            UserName =  user.UserName,
            Email = user.Email,
            Phone = user.PhoneNumber,
            
        };

        return View("GetProfile", uservm);
    }

    #endregion


    #region Edit Profile Get

    [HttpGet]
    public async Task<IActionResult> EditProfile()
    {
        ApplicationUser user = await userManager.GetUserAsync(User);

        if (user == null)
            return RedirectToAction("Login", "Account");

        ProfileViewModel uservm = new ProfileViewModel
        {
            FirstName = user.FirstName,
            LastName  = user.LastName,
            UserName  = user.UserName,
            Email     = user.Email,
            Phone     = user.PhoneNumber
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
        if (!ModelState.IsValid)
        {
            return View("EditProfile", uservm);
        }
        ApplicationUser user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        user.FirstName = uservm.FirstName;
        user. LastName = uservm.LastName;
        user.UserName = uservm.UserName;
        user. Email = uservm.Email;
        user. PhoneNumber = uservm.Phone;
        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View("EditProfile", uservm);
        }

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
        
        await signInManager.SignOutAsync();

        IdentityResult result = await userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            return RedirectToAction("Login", "Account");
        }
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }
        
        return RedirectToAction("GetProfile", "Profile");
    }
    

    #endregion

    #region Add Avatar

    

    #endregion
   
}