namespace MSToolKit.Core.Authentication.Options
{
    /// <summary>
    /// Options for user validation.
    /// </summary>
    public class UserOptions
    {
        private const string DefaultAllowedUserNameCharacters 
            = "@.-_abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public UserOptions()
        {
            this.AllowedUserNameCharacters = DefaultAllowedUserNameCharacters.ToCharArray();
            this.RequireUniqueEmail = true;
        }

        /// <summary>
        /// Gets or sets the list of allowed characters in the username used to validate user names. 
        /// Defaults to @.-_abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789
        /// </summary>
        public char[] AllowedUserNameCharacters { get; set; }

        /// <summary>
        /// Gets a member indicating whether the application requires unique emails for its users. Always true.
        /// </summary>
        public bool RequireUniqueEmail { get; }
    }
}