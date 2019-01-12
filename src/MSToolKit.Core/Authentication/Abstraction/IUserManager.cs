using System.Threading.Tasks;

namespace MSToolKit.Core.Authentication.Abstraction
{
    /// <summary>
    /// Provides an abstraction for managing the user.
    /// </summary>
    /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
    public interface IUserManager<TUser> where TUser : AuthenticationUser
    {
        /// <summary>
        /// Adds the specified user to Administrator role.
        /// </summary>
        /// <param name="user">The user to be added to administrator role.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the MSToolKit.Core.Authentication.AuthenticationResult of the operation.
        /// </returns>
        Task<AuthenticationResult> AddToRoleAdminAsync(TUser user);

        /// <summary>
        /// Changes a user's password after confirming the specified 
        /// currentPassword is correct, as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user whose password should be set.</param>
        /// <param name="currentPassword">The current password to validate before changing.</param>
        /// <param name="newPassword">The new password to set for the specified user.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the MSToolKit.Core.Authentication.AuthenticationResult of the operation.
        /// </returns>
        Task<AuthenticationResult> ChangePasswordAsync(
            TUser user, string currentPassword, string newPassword);

        /// <summary>
        /// Checks the user's credentials against email and password, as an asynchronous operation.
        /// If the user is marked as deleted and credentials are valid - unmarks it.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's password in plain text.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, containing the TUser.
        /// </returns>
        Task<TUser> CheckPasswordAsync(string email, string password);

        /// <summary>
        /// Validates that an email confirmation token matches the specified user.
        /// </summary>
        /// <param name="userId">The user's unique id to validate the token against.</param>
        /// <param name="token">The email confirmation token to validate.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the MSToolKit.Core.Authentication.AuthenticationResult of the operation.
        /// </returns>
        Task<AuthenticationResult> ConfirmEmailAsync(string userId, string token);

        /// <summary>
        /// Creates the specified user in the backing store, as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the MSToolKit.Core.Authentication.AuthenticationResult of the operation.
        /// </returns>
        Task<AuthenticationResult> CreateAsync(TUser user);

        /// <summary>
        /// Marks the specified user as deleted in the backing store, as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user to be deleted.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the MSToolKit.Core.Authentication.AuthenticationResult of the operation.
        /// </returns>
        Task<AuthenticationResult> DeleteAsync(TUser user);

        /// <summary>
        /// Generates an email confirmation token for the specified user.
        /// </summary>
        /// <param name="user">The user to generate an email confirmation token for.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing string representation of the generated token.
        /// </returns>
        Task<string> GenerateEmailConfirmationTokenAsync(TUser user);

        /// <summary>
        /// Generates a password confirmation token for the specified user.
        /// </summary>
        /// <param name="user">The user to generate an email confirmation token for.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing string representation of the generated token.
        /// </returns>
        Task<string> GenerateResetPasswordConfirmationTokenAsync(TUser user);

        /// <summary>
        /// Generates a hash representation of specified password.
        /// </summary>
        /// <param name="password">The password to generate hash for.</param>
        /// <returns>
        /// The generated hash representation of the input.
        /// </returns>
        string GetPasswordHash(string password);

        /// <summary>
        /// Gets the user, if any, associated with the value of the specified email address.
        /// </summary>
        /// <param name="email">The email address to return the user for.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, containing the TUser.
        /// </returns>
        Task<TUser> FindByEmailAsync(string email);

        /// <summary>
        /// Gets the user, if any, associated with the value of the specified unique id.
        /// </summary>
        /// <param name="userId">The user's id to return the user for.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, containing the TUser.
        /// </returns>
        Task<TUser> FindByIdAsync(string userId);

        /// <summary>
        /// Remove the specified user from Administrator role.
        /// </summary>
        /// <param name="user">The user to remove from administrator role.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the MSToolKit.Core.Authentication.AuthenticationResult of the operation.
        /// </returns>
        Task<AuthenticationResult> RemoveFromRoleAdminAsync(TUser user);

        /// <summary>
        /// Updates the specified user in the user store.
        /// </summary>
        /// <param name="user">The user that should be updated.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the MSToolKit.Core.Authentication.AuthenticationResult of the operation.
        /// </returns>
        Task<AuthenticationResult> UpdateAsync(TUser user);

        /// <summary>
        /// Should return MSToolKit.Core.Authentication.AuthenticationResult.Success if validation is successful.
        /// This is called before updating the password hash.
        /// </summary>
        /// <param name="password">The password that should be validated.</param>
        /// <returns>
        /// MSToolKit.Core.Authentication.AuthenticationResult of the operation.
        /// </returns>
        AuthenticationResult ValidatePassword(string password);

        /// <summary>
        /// Sets a user's password as an asynchronous operation.
        /// </summary>
        /// <param name="id">The user identifier, whoch user's password should be set.</param>
        /// <param name="newPassword">The new password to set for the specified user.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the MSToolKit.Core.Authentication.AuthenticationResult of the operation.
        /// </returns>
        Task<AuthenticationResult> SetPasswordAsync(string id, string newPassword);

        /// <summary>
        /// Validates that the password confirmation token matches the specified user.
        /// </summary>
        /// <param name="userId">The user's unique id to validate the token against.</param>
        /// <param name="token">The password confirmation token to validate.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the MSToolKit.Core.Authentication.AuthenticationResult of the operation.
        /// </returns>
        Task<AuthenticationResult> ConfirmResetPasswordTokenAsync(string userId, string code);
    }
}
