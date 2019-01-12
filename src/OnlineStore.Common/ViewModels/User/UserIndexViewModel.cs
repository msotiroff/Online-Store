using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Common.ViewModels.User
{
    public class UserIndexViewModel
    {
        public string Id { get; set; }
        
        public string Email { get; set; }
        
        public string Username { get; set; }

        [Display(Name = "Admin permissions")]
        public string HasAdminPermissions { get; set; }
        
        [Display(Name = "Full name")]
        public string FullName => $"{this.FirstName} {this.LastName}";

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        public bool EmailConfirmed { get; set; }
    }
}
