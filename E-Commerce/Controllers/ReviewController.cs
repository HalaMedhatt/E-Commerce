using E_Commerce.IRepository;
using E_Commerce.Models;
using E_Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class ReviewController : Controller
{
    private readonly IReviewRepository _reviewRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public ReviewController(
        IReviewRepository reviewRepository,
        UserManager<ApplicationUser> userManager)
    {
        _reviewRepository = reviewRepository;
        _userManager = userManager;
    }

    // ================= GET =================
    [HttpGet]
    [Authorize]
    public IActionResult Create(int productId)
    {
        var model = new ReviewViewModel
        {
            ProductId = productId
        };

        return View("Create",model);
    }


    // ================= POST =================
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ReviewViewModel model)
    {
        // لو البيانات غلط
        if (!ModelState.IsValid)
        {
            return View("Create", model);
        }

        // نجيب اليوزر الحالي
        var userId = _userManager.GetUserId(User);

        // أمان زيادة
        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // نكوّن الريفيو
        var review = new ProductReview
        {
            ProductId = model.ProductId,
            Rating = model.Rating,
            Comment = model.Comment,
            UserId = userId,
            CreatedAt = DateTime.Now
        };

        // نحفظ
        _reviewRepository.Add(review);

        // نرجع لصفحة المنتج
        return RedirectToAction(
            "Details",
            "Product",
            new { id = model.ProductId }
        );
    }
}
