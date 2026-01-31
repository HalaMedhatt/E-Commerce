using E_Commerce.Models;

namespace E_Commerce.ViewModels
{
    public class HomeViewModel
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        public int? SelectedCategoryId { get; set; }

    }
}
