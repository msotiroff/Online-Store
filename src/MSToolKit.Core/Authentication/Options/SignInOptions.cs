namespace MSToolKit.Core.Authentication.Options
{
    /// <summary>
    /// Options for configuring sign in.
    /// </summary>
    public class SignInOptions
    {
        public SignInOptions()
        {
            this.RequireConfirmedEmail = false;
        }

        /// <summary>
        /// Gets or sets a member indicating whether a confirmed email address is required to sign in. Defaults to false.
        /// </summary>
        public bool RequireConfirmedEmail { get; set; }
    }
}