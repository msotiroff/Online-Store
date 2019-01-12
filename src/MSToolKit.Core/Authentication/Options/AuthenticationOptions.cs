namespace MSToolKit.Core.Authentication.Options
{
    /// <summary>
    /// Represents all the options you can use to configure the authentication system.
    /// </summary>
    public class AuthenticationOptions
    {
        public AuthenticationOptions(
            UserOptions userOptions, 
            PasswordOptions passwordOptions, 
            SignInOptions signInOptions)
        {
            this.User = userOptions;
            this.Password = passwordOptions;
            this.SignIn = signInOptions;
        }

        public AuthenticationOptions()
            :this(new UserOptions(), new PasswordOptions(), new SignInOptions())
        {
        }

        /// <summary>
        /// Gets or sets the UserOptions for the authentication system.
        /// </summary>
        public UserOptions User { get; set; }

        /// <summary>
        /// Gets or sets the PasswordOptions for the authentication system.
        /// </summary>
        public PasswordOptions Password { get; set; }

        /// <summary>
        /// Gets or sets the SignInOptions for the authentication system.
        /// </summary>
        public SignInOptions SignIn { get; set; }
    }
}
