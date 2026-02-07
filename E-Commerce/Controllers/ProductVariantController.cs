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

            return View(new ProductVariant());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddVariant(int productId, ProductVariant variant)
        {
            ModelState.Remove("Product"); 
            var product = _productRepository.GetById(productId);
            if (product == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    variant.ProductId = productId;

                    _productVariantRepository.Add(variant);
                    _productVariantRepository.Save();

                    TempData["SuccessMessage"] = "New Product Added SuccessFuly";

                    return RedirectToAction("ContinueAdding", new { productId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error Happened: {ex.Message}");
                }
            }

            ViewBag.ProductName = product.Name;
            ViewBag.ProductId = productId;

            return View(variant);
        }
        [HttpGet]
        public IActionResult ContinueAdding(int productId)
        {
            ViewBag.ProductId = productId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAnother(int productId)
        {
            return RedirectToAction("AddVariant", new { productId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Finish(int productId)
        {
            return RedirectToAction("Index", "Product");
        }

        public IActionResult UpdateVrainant(int productId)
        {
            var product = _productRepository.GetById(productId);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.ProductName = product.Name;
            ViewBag.ProductId = productId;

            return View(new ProductVariant());

        }
    }
}
