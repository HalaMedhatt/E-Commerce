using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Product Name is Required")]
        [MaxLength(30, ErrorMessage = "Maximum lenght 30")]
        [MinLength(3, ErrorMessage = "Minimum lenght 3")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;

        //New property 

        public string DefualtImageUrl { get; set; }

        public string BriefDescription { get; set; }


        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<ProductVariant> Variants { get; set; }
        public ICollection<ProductImage> Images { get; set; }
        public ICollection<ProductReview> Reviews { get; set; }
    }
}
