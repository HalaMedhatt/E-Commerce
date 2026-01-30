using E_Commerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers;

public class RoleController : Controller
{
    private readonly RoleManager<IdentityRole> roleManager;

    public RoleController(RoleManager<IdentityRole> roleManager)
    {
        this.roleManager = roleManager;
    }
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Add(RoleViewModel rolevm)
    {
        if (ModelState.IsValid)
        {
            //add db
            IdentityRole roleModel = new IdentityRole()
            {
                Name = rolevm.RoleName
            };

            IdentityResult result=await roleManager.CreateAsync(roleModel);
            if(result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
        }
        return View("Add",rolevm);
    }
}