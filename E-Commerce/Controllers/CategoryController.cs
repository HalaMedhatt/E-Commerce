using E_Commerce.IRepository;
using E_Commerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    public class CategoryController : Controller
    {

        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
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
            Category category = new Category();
            return View("Create",category);

        }
        [HttpPost]
        public IActionResult Save(Category category )
        {
            ModelState.Remove("Products");
            if (!ModelState.IsValid)
            {


                if (category.Id == 0)
                {
                    return View("Create", category);
                }
                else
                {
                    return View("Edit", category);
                }
            }

            if (category.Id == 0)
            {
                _categoryRepository.Add(category);
            }
            else
            {
                _categoryRepository.Edit(category);
            }
            _categoryRepository.Save();
            return RedirectToAction("Index");
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

        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
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
    }
}
