using MSToolKit.Core.Authentication.Abstraction;
using MSToolKit.Core.Authentication.Options;
using MSToolKit.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSToolKit.Core.Authentication
{
    /// <summary>
    /// Provides a default implementation for MSToolKit.Core.Authentication.Abstraction.IUserManager.
    /// </summary>
    /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
    internal class UserManager<TUser> : IUserManager<TUser> where TUser : AuthenticationUser
    {
        private readonly IUserStore<TUser> userStore;
        private readonly IUserValidator<TUser> userValidator;
        private readonly IPasswordHasher passwordHasher;
        private readonly IPasswordValidator passwordValidator;
        private readonly UserOptions userOptions;

        /// <summary>
        /// Initialize a new instance for MSToolKit.Core.Authentication.UserManager.
        /// </summary>
        /// <param name="userStore">
        /// An instance for MSToolKit.Core.Authentication.Abstraction.IUserStore.
        /// </param>
        /// <param name="userValidator">
        /// An instance for MSToolKit.Core.Authentication.Abstraction.IUserValidator.
        /// </param>
        /// <param name="passwordHasher">
        /// An instance for MSToolKit.Core.Authentication.Abstraction.IPasswordHasher.
        /// </param>
        /// <param name="passwordValidator">
        /// An instance for MSToolKit.Core.Authentication.Abstraction.IPasswordValidator.
        /// </param>
        /// <param name="authenticationOptions">
        /// MSToolKit.Core.Authentication.Options.AuthenticationOptions,
        /// that configures the behavior of the current instance.
        /// </param>
        public UserManager(
            IUserStore<TUser> userStore,
            IUserValidator<TUser> userValidator,
            IPasswordHasher passwordHasher,
            IPasswordValidator passwordValidator,
            AuthenticationOptions authenticationOptions)
        {
            this.userStore = userStore;
            this.userValidator = userValidator;
            this.passwordHasher = passwordHasher;
            this.passwordValidator = passwordValidator;
            this.userOptions = authenticationOptions.User;
        }

        /// <summary>
        /// Adds the specified user to Administrator role.
        /// </summary>
        /// <param name="user">The user to be added to administrator role.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the MSToolKit.Core.Authentication.AuthenticationResult of the operation.
        /// </returns>
        public async Task<AuthenticationResult> AddToRoleAdminAsync(TUser user)
        {
            user.IsAdmin = true;
            await this.userStore.SaveAsync(user);

            return new AuthenticationResult(
                true, $"User {user.Email} successfully added to role Admin");
        }

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
        public async Task<AuthenticationResult> ChangePasswordAsync(TUser user, string currentPassword, string newPassword)
        {
            var isMatch = user.PasswordHash == this.GetPasswordHash(currentPassword);
            if (!isMatch)
            {
                return new AuthenticationResult(false, "Wrong password!");
            }

            var passwordValidationResult = this.ValidatePassword(newPassword);
            if (!passwordValidationResult.Success)
            {
                return passwordValidationResult;
            }

            user.PasswordHash = this.GetPasswordHash(newPassword);
            await this.userStore.SaveAsync(user);

            return new AuthenticationResult(true, "Password updated successfully.");
        }

        /// <summary>
        /// Checks the user's credentials against email and password, as an asynchronous operation.
        /// If the user is marked as deleted and credentials are valid - unmarks it.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's password in plain text.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, containing the TUser.
        /// </returns>
        public async Task<TUser> CheckPasswordAsync(string email, string password)
        {
            var passwordHash = this.passwordHasher.ComputeHash(password);
            var user = (await this.userStore
                .FilterAsync(new Dictionary<string, object>
                {
                    [nameof(AuthenticationUser.Email)] = email,
                    [nameof(AuthenticationUser.PasswordHash)] = passwordHash
                }))
                .SingleOrDefault();

            if (user == null)
            {
                return null;
            }

            if (user.IsDeleted)
            {
                user.IsDeleted = false;
                await this.userStore.SaveAsync(user);
            }

            return user;
        }

        /// <summary>
        /// Validates that an email confirmation token matches the specified user.
        /// </summary>
        /// <param name="userId">The user's unique id to validate the token against.</param>
        /// <param name="token">The email confirmation token to validate.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the MSToolKit.Core.Authentication.AuthenticationResult of the operation.
        /// </returns>
        public async Task<AuthenticationResult> ConfirmEmailAsync(string userId, string token)
        {
            var user = await this.FindByIdAsync(userId);
            if (user == null || user.EmailConfirmationToken != token)
            {
                return new AuthenticationResult(false, "Unable to confirm email.");
            }

            user.EmailConfirmed = true;
            await this.userStore.SaveAsync(user);

            return new AuthenticationResult(true, "Email confirmed sucessfully.");
        }

        /// <summary>
        /// Creates the specified user in the backing store, as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the MSToolKit.Core.Authentication.AuthenticationResult of the operation.
        /// </returns>
        public async Task<AuthenticationResult> CreateAsync(TUser user)
        {
            var validationResult = await this.userValidator.ValidateAsync(user);
            if (!validationResult.Success)
            {
                return new AuthenticationResult(false, validationResult.ResponseMessage);
            }

            if (!this.userOptions.RequireUniqueEmail)
            {
                await this.userStore.SaveAsync(user);

                return new AuthenticationResult(true);
            }

            var exist = (await this.userStore
                .FilterAsync(nameof(AuthenticationUser.Email), user.Email))
                .Any();

            if (exist)
            {
                return new AuthenticationResult(
                    false, $"User with email: {user.Email} already exists!");
            }

            await this.userStore.SaveAsync(user);

            return new AuthenticationResult(true);
        }

        /// <summary>
        /// Marks the specified user as deleted in the backing store, as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user to be deleted.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the MSToolKit.Core.Authentication.AuthenticationResult of the operation.
        /// </returns>
        public async Task<AuthenticationResult> DeleteAsync(TUser user)
        {
            await this.userStore.DeleteAsync(user);

            return new AuthenticationResult(true);
        }

        /// <summary>
        /// Gets the user, if any, associated with the value of the specified unique id.
        /// </summary>
        /// <param name="userId">The user's id to return the user for.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, containing the TUser.
        /// </returns>
        public async Task<TUser> FindByIdAsync(string userId)
        {
            var user = await this.userStore.FindByIdAsync(userId);

            return user.IsDeleted ? default(TUser) : user;
        }

        /// <summary>
        /// Generates an email confirmation token for the specified user.
        /// </summary>
        /// <param name="user">The user to generate an email confirmation token for.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing string representation of the generated token.
        /// </returns>
        public async Task<string> GenerateEmailConfirmationTokenAsync(TUser user)
        {
            var token = this.GetConfirmationToken();
            user.EmailConfirmationToken = token;
            await this.userStore.SaveAsync(user);

            return token;
        }

        /// <summary>
        /// Gets the user, if any, associated with the value of the specified email address.
        /// </summary>
        /// <param name="email">The email address to return the user for.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, containing the TUser.
        /// </returns>
        public async Task<TUser> FindByEmailAsync(string email)
        {
            var user = (await this.userStore
                .FilterAsync(nameof(AuthenticationUser.Email), email))
                .SingleOrDefault();

            return user == null
                ? null
                : user.IsDeleted
                    ? null
                    : user;
        }

        /// <summary>
        /// Generates a hash representation of specified password.
        /// </summary>
        /// <param name="password">The password to generate hash for.</param>
        /// <returns>
        /// The generated hash representation of the input.
        /// </returns>
        public string GetPasswordHash(string password)
        {
            return this.passwordHasher.ComputeHash(password);
        }

        /// <summary>
        /// Remove the specified user from Administrator role.
        /// </summary>
        /// <param name="user">The user to remove from administrator role.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the MSToolKit.Core.Authentication.AuthenticationResult of the operation.
        /// </returns>
        public async Task<AuthenticationResult> RemoveFromRoleAdminAsync(TUser user)
        {
            var users = await this.userStore
                .FilterAsync(nameof(AuthenticationUser.IsAdmin), true);

            if (users.Count() == 1)
            {
                return new AuthenticationResult(
                    false, 
                    "Cannot remove the last administrator!");
            }

            user.IsAdmin = false;
            await this.userStore.SaveAsync(user);

            return new AuthenticationResult(true);
        }

        /// <summary>
        /// Updates the specified user in the user store.
        /// </summary>
        /// <param name="updatedUser">The user that should be updated.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the MSToolKit.Core.Authentication.AuthenticationResult of the operation.
        /// </returns>
        public async Task<AuthenticationResult> UpdateAsync(TUser updatedUser)
        {
            var validationResult = updatedUser.GetValidationResult();
            if (!validationResult.Success)
            {
                return new AuthenticationResult(false, validationResult.Errors.First());
            }

            if (this.userOptions.RequireUniqueEmail)
            {
                var existingUser = await this.FindByEmailAsync(updatedUser.Email);
                if (existingUser != null && existingUser.Id != updatedUser.Id)
                {
                    return new AuthenticationResult(
                        false, $"User with email: {updatedUser.Email} already exists!");
                }
            }

            await this.userStore.SaveAsync(updatedUser);

            return new AuthenticationResult(true);
        }

        /// <summary>
        /// Should return MSToolKit.Core.Authentication.AuthenticationResult.Success if validation is successful.
        /// This is called before updating the password hash.
        /// </summary>
        /// <param name="password">The password that should be validated.</param>
        /// <returns>
        /// MSToolKit.Core.Authentication.AuthenticationResult of the operation.
        /// </returns>
        public AuthenticationResult ValidatePassword(string password)
        {
            return this.passwordValidator.Validate(password);
        }

        /// <summary>
        /// Generates a password confirmation token for the specified user.
        /// </summary>
        /// <param name="user">The user to generate an email confirmation token for.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing string representation of the generated token.
        /// </returns>
        public async Task<string> GenerateResetPasswordConfirmationTokenAsync(TUser user)
        {
            var token = this.GetConfirmationToken();
            user.ResetPasswordConfirmationToken = token;
            await this.userStore.SaveAsync(user);

            return token;
        }

        /// <summary>
        /// Sets a user's password as an asynchronous operation.
        /// </summary>
        /// <param name="id">The user identifier, whoch user's password should be set.</param>
        /// <param name="newPassword">The new password to set for the specified user.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the MSToolKit.Core.Authentication.AuthenticationResult of the operation.
        /// </returns>
        public async Task<AuthenticationResult> SetPasswordAsync(string userId, string newPassword)
        {
            var user = await this.FindByIdAsync(userId);
            if (user == null)
            {
                return new AuthenticationResult(false, $"User with Id: {userId} does not exists!");
            }

            var passwordValidation = this.ValidatePassword(newPassword);
            if (!passwordValidation.Success)
            {
                return new AuthenticationResult(false, passwordValidation.ResponseMessage);
            }

            var passwordHash = this.GetPasswordHash(newPassword);
            user.PasswordHash = passwordHash;

            await this.userStore.SaveAsync(user);

            return new AuthenticationResult(true, "Password reset successfully.");
        }

        /// <summary>
        /// Validates that the password confirmation token matches the specified user.
        /// </summary>
        /// <param name="userId">The user's unique id to validate the token against.</param>
        /// <param name="token">The password confirmation token to validate.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the MSToolKit.Core.Authentication.AuthenticationResult of the operation.
        /// </returns>
        public async Task<AuthenticationResult> ConfirmResetPasswordTokenAsync(string userId, string token)
        {
            var user = await this.FindByIdAsync(userId);
            if (user == null || user.ResetPasswordConfirmationToken != token)
            {
                return new AuthenticationResult(false, "Unable to reset password.");
            }
            
            return new AuthenticationResult(true, "Reset password token confirmed sucessfully.");
        }
        
        private string GetConfirmationToken()
        {
            var baseToken = new StringBuilder()
                .Append(Guid.NewGuid().ToString())
                .Append(Guid.NewGuid().ToString())
                .Append(Guid.NewGuid().ToString())
                .ToString();

            return baseToken.Replace("-", string.Empty);
        }
    }
}
