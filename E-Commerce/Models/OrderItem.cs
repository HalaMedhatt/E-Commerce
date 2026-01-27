using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        [Precision(10,2)]
        public decimal UnitPrice { get; set; }
        [Range(0,int.MaxValue)]
        public int Quantity { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; }

    }
}
