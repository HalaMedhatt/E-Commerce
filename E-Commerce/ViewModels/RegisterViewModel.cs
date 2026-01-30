using System.ComponentModel.DataAnnotations;
using E_Commerce.Models;

namespace E_Commerce.ViewModels;

public class RegisterViewModel
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
        
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Compare("Password")]
    [DataType(DataType.Password)]
    [Display(Name ="Confirm Password")]
    public string ConfirmPassword { get; set; }
    
    
    public string PhoneNumber { get; set; }
    
    // public Address Address { get; set; }
    // public string City { get; set; }
}