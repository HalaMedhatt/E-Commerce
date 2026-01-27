using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class ProductVariant
    {
        public int Id { get; set; }
        public string Size { get; set; }

        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
        [Range(0,int.MaxValue)]
        public int StockQuantity { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
