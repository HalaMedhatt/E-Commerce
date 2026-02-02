using E_Commerce.IRepository;
using E_Commerce.Models;
using E_Commerce.Repository;
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

        public IActionResult Index()
        {
            var products = _productRepository.GetAll();  
            var categories = _categoryRepository.GetAll(); 

            var model = new HomeViewModel
            {
                Products = products,
                Categories = categories
            };
            return View(model);

        }

        // GET: ProductController/Details/5
        public IActionResult Details(int id)
        {
            return _productRepository.GetById(id) is var product && product != null
                ? View(product)
                : NotFound();
        }

        //public IActionResult Create()
        //{
        //    return View( new AddNewProductViewModel
        //    {
        //        //Variants = new List<ProductVariant> { new ProductVariant() }
        //    }
        //        );
        //}


        // GET: ProductController/Create
        //[HttpGet]
        //public IActionResult Create()
        //{
        //    Product productModel = new Product();
        //    ViewBag.Categories = new SelectList(
        //        _categoryRepository.GetAll(),
        //        "Id",
        //        "Name"
        //    );
        //    return View("Create", productModel);
        //}







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







        //[HttpPost]

        //public IActionResult Save(AddNewProductViewModel product)
        //{

        //    ModelState.Remove("Variants");
        //    ModelState.Remove("Images");
        //    ModelState.Remove("Category");

        //    ModelState.Remove("Reviews");
        //    if (!ModelState.IsValid)
        //    {

        //        ViewBag.Categories = new SelectList(
        //            _categoryRepository.GetAll(),
        //            "Id",
        //            "Name"
        //        );
        //        //ViewBag.Images = product.Images;
        //        if (product.Id == 0)
        //        {
        //            return View("Create", product);
        //        }
        //        else
        //        {
        //            return View("Edit", product);
        //        }
        //    }

        //    if (product.Id == 0)
        //    {
        //        _productRepository.Add(product);
        //    }
        //    else
        //    {
        //        _productRepository.Edit(product);
        //    }
        //    _productRepository.Save();
        //    return RedirectToAction("Index");
        //}



        //public IActionResult GetProductsByCategory(int categoryId)
        //{
        //    var products = _productRepository.GetAll()
        //                     .Where(p => p.CategoryId == categoryId)
        //                     .ToList();

        //    return View("GetProductsByCategory", products); 
        //}
        // POST: ProductController/Create








        #region Create from deepseek
        //[HttpGet]
        //public IActionResult Create()
        //{
        //    var viewModel = new AddNewProductViewModel
        //    {
        //        Variants = new List<ProductVariant>
        //        {
        //                   new ProductVariant() 
        //        },
        //    };

        //    ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name");
        //    return View(viewModel);
        //}

        //[HttpPost]

        //public IActionResult Create(AddNewProductViewModel viewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var newProduct = new Product
        //            {
        //                Name = viewModel.Name,
        //                Description = viewModel.Description,
        //                CategoryId = viewModel.CategoryId,
        //                IsActive = true,
        //                CreatedAt = DateTime.UtcNow
        //            };

        //            _productRepository.Add(newProduct);
        //            _productRepository.Save(); 

        //            foreach (var variant in viewModel.Variants)
        //            {
        //                if (!string.IsNullOrEmpty(variant.Size) )
        //                {
        //                    var newVariant = new ProductVariant
        //                    {
        //                        Size = variant.Size,
        //                        Price = variant.Price,
        //                        SalePrice = variant.SalePrice,
        //                        StockQuantity = variant.StockQuantity,
        //                        ProductId = newProduct.Id 
        //                    };

        //                    _productVariantRepository.Add(newVariant);
        //                }
        //            }

        //            _productVariantRepository.Save();

        //            TempData["SuccessMessage"] = "Product created successfully!";
        //            return RedirectToAction("Index");
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError("", $"Error: {ex.Message}");

        //            return View(viewModel);
        //        }
        //    }

        //    return View(viewModel);
        //}
        //[HttpPost]
        //public IActionResult Create(Product model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var NewProduct = new Product
        //        {
        //            Name = model.Name,
        //            Description = model.Description,
        //            CategoryId = model.CategoryId,
        //            IsActive = true
        //        };
        //        _productRepository.Add(NewProduct);
        //        _productRepository.Save();
        //        return RedirectToAction("Create", "ProductVariant",NewProduct.Id);
        //    }
        //    return View(model);


        //}
        #endregion


        // GET: ProductController/Edit/5
        // public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        // POST: ProductController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: ProductController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: ProductController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}


        //public IActionResult SearchProducts(string searchTerm)
        //{
        //    if (string.IsNullOrWhiteSpace(searchTerm))
        //        return RedirectToAction("Index");

        //    var products = _productRepository.GetAll()
        //        .Where(p =>
        //            p.Name.Contains(searchTerm) ||
        //            p.Description.Contains(searchTerm))
        //        .ToList();

        //    return View(products);
        //}



        // GET: صفحة إضافة منتج جديد
        [HttpGet]
        public IActionResult Create()
        {
            // جلب التصنيفات من قاعدة البيانات
            var categories = _categoryRepository.GetAll();

            // تحويلها لـ SelectList
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            // إرسال نموذج فارغ
            return View(new Product());
        }

        // POST: حفظ المنتج الجديد
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            ModelState.Remove("Category");
            ModelState.Remove("Variants");
            ModelState.Remove("Images");
            ModelState.Remove("Reviews");
            if (ModelState.IsValid)
            {
                try
                {


                    _productRepository.Add(product);
                    _productRepository.Save();


                    return RedirectToAction("AddVariant", "ProductVariant", new { productId = product.Id });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"❌ حدث خطأ: {ex.Message}");
                }
            }

            // إذا كان النموذج غير صالح، نعيد تعبئة التصنيفات
            var categories = _categoryRepository.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            return View(product);
        }


    }
}
