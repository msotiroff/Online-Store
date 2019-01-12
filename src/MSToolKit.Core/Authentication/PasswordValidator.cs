using MSToolKit.Core.Authentication.Abstraction;
using MSToolKit.Core.Authentication.Options;
using System.Linq;

namespace MSToolKit.Core.Authentication
{
    /// <summary>
    /// Provides a default implementation for MSToolKit.Core.Authentication.Abstraction.IPasswordValidator.
    /// </summary>
    internal class PasswordValidator : IPasswordValidator
    {
        private readonly PasswordOptions passwordOptions;

        /// <summary>
        /// Initialize a new instance of MSToolKit.Core.Authentication.PasswordValidator.
        /// </summary>
        /// <param name="authenticationOptions">
        /// MSToolKit.Core.Authentication.Options.AuthenticationOptions, that configures the behavior of the current instance.
        /// </param>
        public PasswordValidator(AuthenticationOptions authenticationOptions)
        {
            this.passwordOptions = authenticationOptions.Password;
        }

        /// <summary>
        /// Validates the given password.
        /// </summary>
        /// <param name="password">The password (in plain text), that should be validated.</param>
        /// <returns>
        /// MSToolKit.Core.Authentication.AuthenticationResult
        /// </returns>
        public AuthenticationResult Validate(string password)
        {
            if (password.Length < this.passwordOptions.RequiredLength)
            {
                return new AuthenticationResult(
                    false,
                    $"Required password length is {this.passwordOptions.RequiredLength} symbols!");
            }
            if (this.passwordOptions.RequireDigit && !password.Any(ch => char.IsDigit(ch)))
            {
                return new AuthenticationResult(false, "Password must contain at least one digit!");
            }
            if (password.Distinct().ToList().Count() < this.passwordOptions.RequiredUniqueChars)
            {
                return new AuthenticationResult(
                    false, 
                    $"Required unique symbols count is {this.passwordOptions.RequiredUniqueChars}!");
            }
            if (this.passwordOptions.RequireLowercase && !password.Any(ch => char.IsLower(ch)))
            {
                return new AuthenticationResult(
                    false,
                    "Password requires at least one lowercase letter!");
            }
            if (this.passwordOptions.RequireUppercase && !password.Any(ch => char.IsUpper(ch)))
            {
                return new AuthenticationResult(
                    false,
                    "Password requires at least one uppercase letter!");
            }
            if (this.passwordOptions.RequireNonAlphanumeric 
                && !password.Any(ch => char.IsLetterOrDigit(ch)))
            {
                return new AuthenticationResult(
                    false, 
                    "Password requires at least one non alphanumeric symbol!");
            }

            return new AuthenticationResult(true, "Password validation passed.");
        }
    }
}
