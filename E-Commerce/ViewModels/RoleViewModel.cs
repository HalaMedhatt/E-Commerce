using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModels;

public class RoleViewModel
{
    [Display(Name ="Name")]
    public string RoleName { get; set; }
}