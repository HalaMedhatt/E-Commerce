namespace E_Commerce.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public string? UserId { get; set; }
        public string? SessionId { get; set; }
        public ApplicationUser? User { get; set; }
        public bool IsGuestCart => !string.IsNullOrEmpty(SessionId) && string.IsNullOrEmpty(UserId);
        public ICollection<CartItem> CartItems { get; set; }
    }
}
