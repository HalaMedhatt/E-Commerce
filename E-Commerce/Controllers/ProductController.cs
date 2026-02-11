using E_Commerce.IRepository;
using E_Commerce.Models;
using E_Commerce.Repository;
using E_Commerce.ViewModels;
using E_Commerce.ViewModels.ProductVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace E_Commerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IReviewRepository _reviewRepository;
        public ProductController(IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IWebHostEnvironment webHostEnvironment,
            IProductVariantRepository productVariantRepository,
            IReviewRepository reviewRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
            _productVariantRepository = productVariantRepository;
            _reviewRepository = reviewRepository;
        }

        //public IActionResult Index()
        //{
        //    var products = _productRepository.GetAll();  
        //    var categories = _categoryRepository.GetAll(); 

        //    var model = new HomeViewModel
        //    {
        //        Products = products,
        //        Categories = categories
        //    };
        //    return View(model);

        //}
        public IActionResult Index(string? search, int page = 1)
        {
            int pageSize = 12; // عدد المنتجات لكل صفحة

            var model = new HomeViewModel();

            // جلب كل التصنيفات
            model.Categories = _categoryRepository.GetAll();

            var productsQuery = _productRepository.GetAll().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                productsQuery = productsQuery.Where(x =>
                    (x.Name != null && x.Name.ToLower().Contains(search.ToLower())) ||
                    (x.BriefDescription != null && x.BriefDescription.ToLower().Contains(search.ToLower()))
                );
            }

            // حساب pagination
            int totalProducts = productsQuery.Count();
            int totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            var products = productsQuery
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();

            model.Products = products;

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.Search = search;

            return View(model);
        }





        public IActionResult Details(int id)
        {
            var product = _productRepository.GetById(id);

            if (product == null)
                return NotFound();

            var productReviews = _reviewRepository.GetReviewsByProductId(id).ToList();

            var viewModel = new ProductDetailsViewModel
            {
                Product = product,
                Reviews = productReviews
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult UpdateProduct(int id)
        {
            var categories = _categoryRepository.GetAll();

            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            return _productRepository.GetById(id) is var product && product != null
                ? View(product)
                : NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateProduct(int id, Product model, IFormFile image)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
                return NotFound();

            ModelState.Remove("Category");
            ModelState.Remove("Variants");
            ModelState.Remove("Images");
            ModelState.Remove("Reviews");
            ModelState.Remove("DefualtImageUrl");
            ModelState.Remove("Image");

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(
                    _categoryRepository.GetAll(), "Id", "Name"
                );
                return View(model);
            }


            if (image != null && image.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    image.CopyTo(stream);
                }

                product.DefualtImageUrl = fileName;
            }

       

            product.Name = model.Name;
            product.Description = model.Description;
            product.BriefDescription = model.BriefDescription;
            product.IsActive = model.IsActive;
            product.CategoryId = model.CategoryId; 

            _productRepository.Edit(product);
            _productRepository.Save();

            return RedirectToAction("IndexAdmin","Home", new { id = product.Id });
        }






        [HttpGet]
        public IActionResult Create()
        {
            var categories = _categoryRepository.GetAll();

            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product, IFormFile image)
        {
            ModelState.Remove("Category");
            ModelState.Remove("Variants");
            ModelState.Remove("Images");
            ModelState.Remove("Reviews");
            ModelState.Remove("DefualtImageUrl");

            if (image == null || image.Length == 0)
            {
                ModelState.AddModelError("DefualtImageUrl", "Product image is required");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                    var filePath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot/images",
                        fileName
                    );

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }

                    product.DefualtImageUrl = fileName;

                    _productRepository.Add(product);
                    _productRepository.Save();

                    return RedirectToAction(
                        "AddVariant",
                        "ProductVariant",
                        new { productId = product.Id }
                    );
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ViewBag.Categories = new SelectList(
                _categoryRepository.GetAll(), "Id", "Name"
            );

            return View(product);
        }
        


    }
}
