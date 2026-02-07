using E_Commerce.Models.Enum;

namespace E_Commerce.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string TransactionRef { get; set; } = string.Empty;

		public DateTime PaymentDate { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
