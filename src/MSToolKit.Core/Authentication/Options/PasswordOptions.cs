namespace MSToolKit.Core.Authentication.Options
{
    /// <summary>
    /// Specifies options for password requirements.
    /// </summary>
    public class PasswordOptions
    {
        public PasswordOptions()
        {
            this.RequiredLength = 4;
            this.RequiredUniqueChars = 1;
            this.RequireNonAlphanumeric = false;
            this.RequireLowercase = false;
            this.RequireUppercase = false;
            this.RequireDigit = false;
        }

        /// <summary>
        /// Gets or sets the minimum length a password must be. Defaults to 4.
        /// </summary>
        public int RequiredLength { get; set; }

        /// <summary>
        /// Gets or sets the minimum number of unique chars a password must comprised of. Defaults to 1.
        /// </summary>
        public int RequiredUniqueChars { get; set; }

        /// <summary>
        /// Gets or sets a member indicating if passwords must contain a non-alphanumeric character. Defaults to false.
        /// </summary>
        public bool RequireNonAlphanumeric { get; set; }

        /// <summary>
        /// Gets or sets a member indicating if passwords must contain a lower case ASCII character. Defaults to false.
        /// </summary>
        public bool RequireLowercase { get; set; }

        /// <summary>
        /// Gets or sets a member indicating if passwords must contain an upper case ASCII character. Defaults to false.
        /// </summary>
        public bool RequireUppercase { get; set; }

        /// <summary>
        /// Gets or sets a member indicating if passwords must contain a digit. Defaults to false.
        /// </summary>
        public bool RequireDigit { get; set; }
    }
}