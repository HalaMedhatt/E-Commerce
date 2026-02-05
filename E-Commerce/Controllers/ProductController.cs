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

namespace E_Commerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProductVariantRepository _productVariantRepository;
        public ProductController(IProductRepository productRepository, 
            ICategoryRepository categoryRepository,
            IWebHostEnvironment webHostEnvironment,
            IProductVariantRepository productVariantRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
            _productVariantRepository = productVariantRepository;
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
        public IActionResult Index(string? search)
        {
            var model = new HomeViewModel();

            model.Categories = _categoryRepository.GetAll() ;
            var productsQuery = _productRepository.GetAll().AsQueryable() ;




            if (!string.IsNullOrWhiteSpace(search))
            {
                productsQuery = productsQuery.Where(x =>
                    x.Name != null &&
                    x.BriefDescription != null &&
                    (
                        x.Name.ToLower().Contains(search.ToLower()) ||
                        x.BriefDescription.ToLower().Contains(search.ToLower())
                    )
                );
            }


            model.Products = productsQuery.ToList();

            return View(model);
        }

        public IActionResult Details(int id)
        {
            return _productRepository.GetById(id) is var product && product != null
                ? View(product)
                : NotFound();
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
