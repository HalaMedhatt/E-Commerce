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
        public int CategoryId { get; set; }
        //public List<Category> Categories { get; set; }
        public List<ProductVariant> Variants { get; set; }
        public List<ProductImage> Images { get; set; }

        //public string Size { get; set; }

        //public decimal Price { get; set; }
        //public decimal? SalePrice { get; set; }
        //public int StockQuantity { get; set; }



    }
}
