using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Category Name is Required")]
        [MaxLength(30, ErrorMessage = "Maximum lenght 30")]
        [MinLength(3, ErrorMessage = "Minimum lenght 3")]
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;

        public ICollection<Product> Products { get; set; }
    }
}
