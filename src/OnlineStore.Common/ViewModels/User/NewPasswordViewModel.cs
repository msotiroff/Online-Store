using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Common.ViewModels.User
{
    public class NewPasswordViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "New password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        [Display(Name = "Confirm new password")]
        public string ConfirmNewPassword { get; set; }
    }

}
