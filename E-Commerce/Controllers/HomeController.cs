using E_Commerce.IRepository;
using E_Commerce.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace E_Commerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IReviewRepository _reviewRepository;
        public HomeController(IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IWebHostEnvironment webHostEnvironment,
            IReviewRepository reviewRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
            _reviewRepository = reviewRepository;
        }
        public IActionResult Index() 
        { 
            var categories = _categoryRepository.GetAll();
            ViewBag.Products = _productRepository.GetAll();
            ViewBag.Reviews = _reviewRepository.GetAll();
            return View(categories); 
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult IndexAdmin(int page = 1)
        {
            int pageSize = 5;

            var allProducts = _productRepository.GetAll();

            int totalProducts = allProducts.Count();
            int totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            var products = allProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.Products = products;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            var categories = _categoryRepository.GetAll();
            return View(categories);
        }

        public IActionResult Categories()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
