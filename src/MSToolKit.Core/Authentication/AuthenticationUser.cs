using MSToolKit.Core.DataAccess.Abstraction;
using System;
using System.ComponentModel.DataAnnotations;

namespace MSToolKit.Core.Authentication
{
    /// <summary>
    /// The default implementation of MSToolKit.Core.Authentication.AuthenticationUser, which uses a string as a primary key.
    /// </summary>
    public class AuthenticationUser : IEntity<string>
    {
        /// <summary>
        /// Initializes a new instance of MSToolKit.Core.Authentication.AuthenticationUser.
        /// The Id property is initialized to form a new GUID string value.
        /// </summary>
        /// <param name="email">The specified user's email.</param>
        /// <param name="passwordHash">The hash representation of the user's password.</param>
        public AuthenticationUser(string email, string passwordHash)
        {
            this.Id = Guid.NewGuid().ToString();
            this.Email = email;
            this.Username = email;
            this.PasswordHash = passwordHash;
            this.EmailConfirmed = false;
            this.IsAdmin = false;
            this.EmailConfirmationToken = default(string);
            this.ResetPasswordConfirmationToken = default(string);
        }

        /// <summary>
        /// Initializes a new instance of MSToolKit.Core.Authentication.AuthenticationUser.
        /// The Id property is initialized to form a new GUID string value.
        /// </summary>
        /// <param name="email">The specified user's email.</param>
        /// <param name="username">The specified user's username.</param>
        /// <param name="passwordHash">The hash representation of the user's password.</param>
        public AuthenticationUser(string email, string username, string passwordHash)
            : this(email, passwordHash)
        {
            this.Username = username;
        }

        /// <summary>
        /// Initializes a new instance of MSToolKit.Core.Authentication.AuthenticationUser.
        /// The Id property is initialized to form a new GUID string value.
        /// </summary>
        public AuthenticationUser()
        {
        }

        /// <summary>
        /// Gets or sets the primary key for this user.
        /// </summary>
        [Key]
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the email address for this user.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the username for this user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the hash representation of this user's password.
        /// </summary>
        [Required]
        public string PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets the member, that indicates wether the user's email is confirmed ot not.
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// Gets or sets the member, that indicates wether the user has administrator control ot not.
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Gets or sets the member, that indicates wether the user is marked as deleted ot not.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the email confirmation token for this user.
        /// </summary>
        public string EmailConfirmationToken { get; set; }

        /// <summary>
        /// Gets or sets the reset password confirmation token for this user.
        /// </summary>
        public string ResetPasswordConfirmationToken { get; set; }
    }
}
