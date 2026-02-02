using E_Commerce.IRepository;
using E_Commerce.Models;
using E_Commerce.Repository;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    public class ProductVariantController : Controller
    {
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IProductRepository _productRepository;
        public ProductVariantController(IProductVariantRepository productVariantRepository,
            IProductRepository productRepository)
        {
            _productVariantRepository = productVariantRepository;
            _productRepository = productRepository;
        }

        //public IActionResult Index()
        //{


        //    return View();
        //}
        //[HttpGet]
        //    public IActionResult Create(int productId)
        //    {
        //        ViewBag.ProductId = productId;
        //        return View();
        //    }


        //    [HttpPost]
        //    public IActionResult Create(int productId , ProductVariant productVariant)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            try
        //            {
        //                var newVariant = new ProductVariant
        //                {
        //                    Size = productVariant.Size,
        //                    Price = productVariant.Price,
        //                    SalePrice = productVariant.SalePrice,
        //                    StockQuantity = productVariant.StockQuantity,
        //                    ProductId = productId
        //                };
        //                _productVariantRepository.Add(newVariant);
        //                _productVariantRepository.Save();
        //                return RedirectToAction("Index");
        //            }
        //            catch (Exception ex)
        //            {

        //                ModelState.AddModelError("exception", ex.InnerException.Message);
        //            }
        //        }

        //            return View(productVariant);
        //    }



        [HttpGet]
        public IActionResult AddVariant(int productId)
        {
            var product = _productRepository.GetById(productId);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.ProductName = product.Name;
            ViewBag.ProductId = productId;

            // إنشاء نموذج جديد
            return View(new ProductVariant());
        }

        // POST: حفظ المتغيرات
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddVariant(int productId, ProductVariant variant)
        {
            ModelState.Remove("Product"); // إزالة التحقق من صحة معرف المتغير
            // البحث عن المنتج
            var product = _productRepository.GetById(productId);
            if (product == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // ربط المتغير بالمنتج
                    variant.ProductId = productId;

                    // إضافة المتغير
                    _productVariantRepository.Add(variant);
                    _productVariantRepository.Save();

                    // رسالة نجاح
                    TempData["SuccessMessage"] = "✅ تم إضافة المتغير بنجاح!";

                    // عرض خيارات للمستخدم
                    return RedirectToAction("ContinueAdding", new { productId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"❌ حدث خطأ: {ex.Message}");
                }
            }

            // في حالة الخطأ، إعادة تعبئة البيانات
            ViewBag.ProductName = product.Name;
            ViewBag.ProductId = productId;

            return View(variant);
        }
        // GET: صفحة الاستمرار في الإضافة
        [HttpGet]
        public IActionResult ContinueAdding(int productId)
        {
            ViewBag.ProductId = productId;
            return View();
        }

        // POST: إضافة متغير آخر
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAnother(int productId)
        {
            return RedirectToAction("AddVariant", new { productId });
        }

        // POST: إنهاء وعرض المنتجات
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Finish(int productId)
        {
            return RedirectToAction("Index", "Product");
        }


    }
}
