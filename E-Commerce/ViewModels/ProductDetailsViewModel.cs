using E_Commerce.Models;

namespace E_Commerce.ViewModels
{
    public class ProductDetailsViewModel
    {
            public Product Product { get; set; }

            public List<ProductReview> Reviews { get; set; } 
        
    }
}
