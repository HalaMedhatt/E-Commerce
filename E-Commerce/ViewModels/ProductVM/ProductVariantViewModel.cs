using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModels.ProductVM
{
    public class ProductVariantViewModel
    {
        public string Size { get; set; }

        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
        public int StockQuantity { get; set; }

    }
}
