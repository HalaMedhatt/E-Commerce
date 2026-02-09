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


    [HttpGet]
    public IActionResult Index(int productId)
    {
        var reviews = _reviewRepository.GetReviewsByProductId(productId).ToList();

        ViewBag.ProductId = productId;

        return View("_IndexRewiewPartial", reviews);
    }


    [HttpGet]
    [Authorize]
    public IActionResult Create(int productId)
    {
        var model = new ReviewViewModel
        {
            ProductId = productId
        };

        return View("_AddReviewsPartial", model);
    }


    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ReviewViewModel model)
    {
        ModelState.Remove("UserName");
        if (!ModelState.IsValid)
        {
            return View("_AddReviewsPartial", model);
        }

        var userId = _userManager.GetUserId(User);

        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var review = new ProductReview
        {
            ProductId = model.ProductId,
            Rating = model.Rating,
            Comment = model.Comment,
            UserId = userId,
            CreatedAt = DateTime.Now
        };

        _reviewRepository.Add(review);

        return RedirectToAction(
            "Details",
            "Product",
            new { id = model.ProductId }
        );
    }
}
