using System.Threading.Tasks;

namespace MSToolKit.Core.Authentication.Abstraction
{
    /// <summary>
    /// Provides an abstraction for user validation.
    /// </summary>
    /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
    public interface IUserValidator<TUser> where TUser : AuthenticationUser
    {
        /// <summary>
        /// Validates the specified user as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user to validate.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the MSToolKit.Core.Authentication.AuthenticationResult of the operation.
        /// </returns>
        Task<AuthenticationResult> ValidateAsync(TUser user);
    }
}
