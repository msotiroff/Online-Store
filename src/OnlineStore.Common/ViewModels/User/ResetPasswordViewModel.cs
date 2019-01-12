using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Common.ViewModels.User
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
