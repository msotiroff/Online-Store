using MSToolKit.Core.Authentication;
using OnlineStore.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class User : AuthenticationUser
    {
        public User()
        {
        }

        public User(string email, string passwordHash, string firstName, string lastName)
            : base(email, passwordHash)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public string FullName => $"{this.FirstName} {this.LastName}";

        [Required]
        [MinLength(ModelConstants.UserNameMinLength)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(ModelConstants.UserNameMinLength)]
        public string LastName { get; set; }
    }
}