using E_Commerce.Models;
using E_Commerce.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModels
{
    public class CheckoutViewModel
    {
        public int ShippingAddressId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public List<Address> Addresses { get; set; }=new List<Address>();

    }
}
