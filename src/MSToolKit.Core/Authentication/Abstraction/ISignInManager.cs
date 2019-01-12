using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MSToolKit.Core.Authentication.Abstraction
{
    /// <summary>
    /// Provides an abstraction for user sign in.
    /// </summary>
    /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
    public interface ISignInManager<TUser> where TUser : AuthenticationUser
    {
        /// <summary>
        /// Returns true or false depending on whether it is mandatory for 
        /// the user to confirm his/her email, in order to sign in.
        /// </summary>
        bool RequireConfirmedEmail { get; }

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
        Task<AuthenticationResult> SignInAsync(
            HttpContext httpContext, string email, string passwordHash, bool isPersistent = false);

        /// <summary>
        /// Signs the current user out of the application.
        /// </summary>
        /// <param name="httpContext">Microsoft.AspNetCore.Http.HttpContext.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation.
        /// </returns>
        Task SignOutAsync(HttpContext httpContext);
    }
}