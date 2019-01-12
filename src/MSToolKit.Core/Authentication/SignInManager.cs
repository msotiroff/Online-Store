using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using MSToolKit.Core.Authentication.Abstraction;
using MSToolKit.Core.Authentication.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MSToolKit.Core.Authentication
{
    /// <summary>
    /// Provides a default implementattion for MSToolKit.Core.Authentication.Abstraction.ISignInManager
    /// </summary>
    /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
    internal class SignInManager<TUser> : ISignInManager<TUser> where TUser : AuthenticationUser
    {
        private readonly IUserManager<TUser> userManager;
        private readonly SignInOptions signInOptions;

        /// <summary>
        /// Initialize a new instance of MSToolKit.Core.Authentication.SignInManager
        /// </summary>
        /// <param name="userManager">
        /// An instance for MSToolKit.Core.Authentication.Abstraction.IUserManager.
        /// </param>
        /// <param name="authenticationOptions">
        /// MSToolKit.Core.Authentication.Options.AuthenticationOptions,
        /// that configures the behavior of the current instance.
        /// </param>
        public SignInManager(
            IUserManager<TUser> userManager,
            Options.AuthenticationOptions authenticationOptions)
        {
            this.userManager = userManager;
            this.signInOptions = authenticationOptions.SignIn;
        }

        /// <summary>
        /// Gets the member, that indicates wether the email confirmation is required for the user in order to sign in. Default value: false.
        /// </summary>
        public bool RequireConfirmedEmail => this.signInOptions.RequireConfirmedEmail;

        /// <summary>
        /// Signs the user with specified email and password.
        /// </summary>
        /// <param name="httpContext">Microsoft.AspNetCore.Http.HttpContext.</param>
        /// <param name="email">Email address.</param>
        /// <param name="passwordHash">Hashed representation of the user's password.</param>
        /// <param name="isPersistent">Indicates wether the user session should be persistent(Default value: false).</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the MSToolKit.Core.Authentication.AuthenticationResult of the operation.
        /// </returns>
        public async Task<AuthenticationResult> SignInAsync(
            HttpContext httpContext, string email, string password, bool isPersistent = true)
        {
            var dbUser = await this.userManager.CheckPasswordAsync(email, password);
            if (dbUser == null)
            {
                return new AuthenticationResult(false, "Invalid login attempt!");
            }
            if (!dbUser.EmailConfirmed && this.signInOptions.RequireConfirmedEmail)
            {
                return new AuthenticationResult(
                    false, "Please, confirm your email before sign in!");
            }

            var identity = new ClaimsIdentity(
                this.GetAuthenticationUserClaims(dbUser),
                CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = isPersistent
            };

            await httpContext
                .SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme, 
                    principal, 
                    authProperties);

            return new AuthenticationResult(true, "Login successfull.");
        }

        /// <summary>
        /// Signs the current user out of the application.
        /// </summary>
        /// <param name="httpContext">Microsoft.AspNetCore.Http.HttpContext.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation.
        /// </returns>
        public async Task SignOutAsync(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
        }

        private IEnumerable<Claim> GetAuthenticationUserClaims(AuthenticationUser user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(AuthenticationConstants.ClaimTypeIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.IsAdmin
                    ? AuthenticationConstants.AdministratorRoleName 
                    : AuthenticationConstants.RegularRoleName)
            };

            return claims;
        }
    }
}