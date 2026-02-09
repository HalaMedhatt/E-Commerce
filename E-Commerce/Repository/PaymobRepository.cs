using E_Commerce.IRepository;
using E_Commerce.Models;
using Newtonsoft.Json;
using System.Text;

namespace E_Commerce.Repository
{
    public class PaymobRepository (IConfiguration _configuration, HttpClient _httpClient) : IPaymobRepository
    {
		public async Task<(string paymobOrderId, string paymentToken)> GetPaymentTokenAsync(Order order, Payment payment)
		{
			var authToken = await GetAuthTokenAsync();
			var paymobOrderId = await RegisterOrderAsync(authToken, order);
			var paymentToken = await GetPaymentKeyAsync(authToken, order, paymobOrderId, payment);
			return (paymobOrderId, paymentToken);
		}
		private async Task<string> GetAuthTokenAsync()
		{
			var apiKey = _configuration["Paymob:ApiKey"];
			var request = new
			{
				api_key = apiKey
			};

			var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync("https://accept.paymob.com/api/auth/tokens", content);
			response.EnsureSuccessStatusCode();

			var responseString = await response.Content.ReadAsStringAsync();
			dynamic data = JsonConvert.DeserializeObject(responseString);
			return data.token;
		}
		private async Task<string> RegisterOrderAsync(string authToken, Order order)
		{
			var request = new
			{
				auth_token = authToken,
				delivery_needed = "false",
				amount_cents = (int)(order.TotalCost * 100), // تحويل المبلغ إلى قروش
				currency = "EGP",
				items = new object[] { } // يمكن إضافة تفاصيل العناصر هنا إذا لزم الأمر
			};

			var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync("https://accept.paymob.com/api/ecommerce/orders", content);
			response.EnsureSuccessStatusCode();

			var responseString = await response.Content.ReadAsStringAsync();
			dynamic data = JsonConvert.DeserializeObject(responseString);
			return data.id.ToString();
		}

		private async Task<string> GetPaymentKeyAsync(string authToken, Order order, string orderId, Payment payment)
		{
			var integrationId = _configuration["Paymob:IntegrationId"];
			var request = new
			{
				auth_token = authToken,
				amount_cents = (int)(order.TotalCost * 100),
				expiration = 3600, // صلاحية الرمز بالثواني
				order_id = orderId,
				billing_data = new
				{
					apartment = "NA",
					email = order.User.Email,
					floor = "NA",
					first_name = order.User.FirstName,
					street = "NA",
					building = "NA",
					phone_number = order.User.PhoneNumber,
					shipping_method = "NA",
					postal_code = "NA",
					city = "NA",
					country = "NA",
					last_name = order.User.LastName,
					state = "NA"
				},
				currency = "EGP",
				integration_id = integrationId
			};

			var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync("https://accept.paymob.com/api/acceptance/payment_keys", content);
			response.EnsureSuccessStatusCode();

			var responseString = await response.Content.ReadAsStringAsync();
			dynamic data = JsonConvert.DeserializeObject(responseString);
			return data.token;
		}
		public async Task<bool> ConfirmPaymentAsync(string paymentToken, decimal amount)
        {
			// يمكن استخدام هذه الطريقة للتحقق من حالة الدفع إذا لزم الأمر
			// قد تحتاج إلى تعديلها بناءً على متطلبات Paymob
			return await Task.FromResult(true);
		}

    }
}
