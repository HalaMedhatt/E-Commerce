using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class Address
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="City is Required")]
        public string City { get; set; }
        [Required(ErrorMessage ="Street is Required")]
        public string Street { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
