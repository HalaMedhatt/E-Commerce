using E_Commerce.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;
        [Precision(10,2)]
        public decimal TotalCost { get; set; }
        public OrderStatus Status { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int ShippingAddressId { get; set; }
        public Address ShippingAddress { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
        public Payment Payment { get; set; }
    }
}
