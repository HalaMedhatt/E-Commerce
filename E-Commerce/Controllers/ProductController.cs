using E_Commerce.IRepository;
using E_Commerce.Models;
using E_Commerce.ViewModels;
using E_Commerce.ViewModels.ProductVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace E_Commerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IProductRepository productRepository, 
            ICategoryRepository categoryRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var products = _productRepository.GetAll();  // كل المنتجات
            var categories = _categoryRepository.GetAll(); // كل التصنيفات

            var model = new HomeViewModel
            {
                Products = products,
                Categories = categories
            };
            return View(model);

        }

        public IActionResult GetProductsByCategory(int categoryId)
        {
            var products = _productRepository.GetAll()
                             .Where(p => p.CategoryId == categoryId)
                             .ToList();

            return View("GetProductsByCategory", products); 
        }
        // GET: ProductController/Details/5
        public IActionResult Details(int id)
        {
            return _productRepository.GetById(id) is var product && product != null
                ? View(product)
                : NotFound();
        }

        // GET: ProductController/Create
        [HttpGet]
        public IActionResult Create()
        {
            Product productModel = new Product();
            ViewBag.Categories = new SelectList(
                _categoryRepository.GetAll(),
                "Id",
                "Name"
            );
            return View("Create",productModel);
        }

        // POST: ProductController/Create
        [HttpPost]

        public IActionResult Save(Product product)
        {

            ModelState.Remove("Variants");
            ModelState.Remove("Images");
            ModelState.Remove("Category");

            ModelState.Remove("Reviews");
            if (!ModelState.IsValid)
            {

                ViewBag.Categories = new SelectList(
                    _categoryRepository.GetAll(),
                    "Id",
                    "Name"
                );
                //ViewBag.Images = product.Images;
                if (product.Id == 0)
                {
                    return View("Create", product);
                }
                else
                {
                    return View("Edit", product);
                }
            }

            if (product.Id == 0)
            {
                _productRepository.Add(product);
            }
            else
            {
                _productRepository.Edit(product);
            }
            _productRepository.Save();
            return RedirectToAction("Index");
        }
        //public ActionResult Create(AddNewProductViewModel productModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        productModel.Categories = _categoryRepository.GetAll().ToList();
        //        return View("Create", productModel);
        //    }
        //    var newProduct = new Product
        //    {
        //        Name = productModel.Name,
        //        Description = productModel.Description,
        //        IsActive = productModel.IsActive,
        //        CreatedAt = DateTime.Now,
        //        CategoryId = productModel.CategoryId

        //    };



        //}








        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        public IActionResult SearchProducts(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return RedirectToAction("Index");

            var products = _productRepository.GetAll()
                .Where(p =>
                    p.Name.Contains(searchTerm) ||
                    p.Description.Contains(searchTerm))
                .ToList();

            return View(products);
        }

    }
}
