using E_Commerce.IRepository;
using E_Commerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace E_Commerce.Controllers
{
    public class CategoryController : Controller
    {

        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        public CategoryController(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }
        public ActionResult Index()
        {
            var categories = _categoryRepository.GetAll();
            return View(categories);
        }

        // GET: CategoryController/Details/5
        public IActionResult Details(int id)
        {
            var category = _categoryRepository.GetById(id);
            return View(category);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            ModelState.Remove("Products");

            if (!ModelState.IsValid)
                return View(category);

            _categoryRepository.Add(category);
            _categoryRepository.Save();

            return RedirectToAction("IndexAdmin", "Home");
        }


        // GET: CategoryController/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
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
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var category = _categoryRepository.GetById(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        public bool CategoryHasProducts(int categoryId)
        {
            return _productRepository.GetAll().Any(p=> p.CategoryId == categoryId);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _categoryRepository.GetById(id);

            if (category == null)
                return NotFound();

            if (CategoryHasProducts(id))
            {
                TempData["Error"] = "You can't delete this category because it contains products.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            try
            {
                _categoryRepository.Delete(id);
                _categoryRepository.Save();
            }
            catch
            {
                TempData["Error"] = "Something went wrong while deleting the category.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction("IndexAdmin", "Home");
        }


    }
}
