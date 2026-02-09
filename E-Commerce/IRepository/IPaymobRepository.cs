using E_Commerce.Models;

namespace E_Commerce.IRepository
{
    public interface IPaymobRepository
	{
		Task<(string paymobOrderId, string paymentToken)> GetPaymentTokenAsync(Order order, Payment payment);
		Task<bool> ConfirmPaymentAsync(string paymentToken, decimal amount);
	}
}
