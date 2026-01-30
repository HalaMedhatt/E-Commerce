using E_Commerce.Models;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModels.ProductVM
{
    public class AddNewProductViewModel
    {
        [Required(ErrorMessage = "Product Name is Required")]
        [MaxLength(30, ErrorMessage = "Maximum lenght 30")]
        [MinLength(3, ErrorMessage = "Minimum lenght 3")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int CategoryId { get; set; }
        public List<Category> Categories { get; set; }



    }
}
