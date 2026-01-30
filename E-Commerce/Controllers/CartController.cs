using E_Commerce.IRepository;
using E_Commerce.Repository;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    public class CartController(ICartRepository cartRepository) : Controller
    {
        //public IActionResult Index()
    }
}
