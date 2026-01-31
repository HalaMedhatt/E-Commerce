using E_Commerce;
using E_Commerce.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class ReviewController : Controller
{
    private readonly ECommerceDbContext _context;

    public ReviewController(ECommerceDbContext context)
    {
        _context = context;
    }

    // فتح صفحة الإضافة
    [HttpGet]
    public IActionResult Create(int productId)
    {
        var review = new ProductReview
        {
            ProductId = productId
        };

        return View(review);
    }

    // استقبال الفورم
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ProductReview review)
    {
        if (ModelState.IsValid)
        {
            if (User.Identity.IsAuthenticated)
            {
                review.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            _context.ProductReviews.Add(review);
            _context.SaveChanges();

            return RedirectToAction("Details", "Product", new { id = review.ProductId });
        }

        return View(review);
    }
}
