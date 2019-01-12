using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Common.ViewModels.User
{
    public class UserAdminViewModel
    {
        public string Id { get; set; }
        
        public string Email { get; set; }
        
        public string Username { get; set; }
        
        [Display(Name = "Administrator permissions")]
        public bool IsAdmin { get; set; }

        [Display(Name = "Full name")]
        public string FullName { get; set; }
    }
}
