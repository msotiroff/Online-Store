using OnlineStore.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Common.ViewModels.User
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        [MinLength(ModelConstants.UserNameMinLength)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(ModelConstants.UserNameMinLength)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
    }
}
