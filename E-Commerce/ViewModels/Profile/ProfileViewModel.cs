using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModels;

public class ProfileViewModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; }
    public string? Avatar { get; set; }
    public List<string> Avatars { get; set; }
    public DateTime? DateJoined { get; set; }
    


}