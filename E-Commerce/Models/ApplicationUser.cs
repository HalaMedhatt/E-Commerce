using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace E_Commerce.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required(ErrorMessage ="First Name is Required")]
        [MaxLength(10,ErrorMessage ="Maximum lenght 10")]
        [MinLength(3,ErrorMessage ="Minimum lenght 3")]
        public string FirstName { get; set; }
        [Required(ErrorMessage ="Last Name is Required")]
        [MaxLength(10, ErrorMessage = "Maximum lenght 10")]
        [MinLength(3, ErrorMessage = "Minimum lenght 3")]
        public string LastName { get; set; }

        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;

        public ICollection<Address> Addresses { get; set; }
        public ICollection<Order> Orders { get; set; }
        public Cart Cart { get; set; }
        public ICollection<ProductReview> Reviews { get; set; }
    }
}
