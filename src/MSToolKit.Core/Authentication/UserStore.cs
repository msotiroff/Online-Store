using System.Collections.Generic;
using System.Threading.Tasks;
using MSToolKit.Core.Authentication.Abstraction;
using MSToolKit.Core.DataAccess.Abstraction;

namespace MSToolKit.Core.Authentication
{
    /// <summary>
    /// Provides a default implementation for MSToolKit.Core.Authentication.Abstraction.IUserStore.
    /// </summary>
    /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
    internal class UserStore<TUser> : IUserStore<TUser> where TUser : AuthenticationUser
    {
        private readonly IDbContext<TUser, string> dbContext;

        /// <summary>
        /// Initialize a new instance for MSToolKit.Core.Authentication.UserStore.
        /// </summary>
        /// <param name="dbContext">
        /// An instance for MSToolKit.Core.DataAccess.Abstraction.IDbContext
        /// </param>
        public UserStore(IDbContext<TUser, string> dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Marks the specified user as deleted.
        /// </summary>
        /// <param name="user">The user to be marked as deleted.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation.
        /// </returns>
        public async Task DeleteAsync(TUser user)
        {
            user.IsDeleted = true;
            await this.dbContext.SaveAsync(user);
        }

        /// <summary>
        /// Finds and returns users, if any, who have the specified member with the specified value.
        /// </summary>
        /// <param name="propertyName">The member, which value should match in the search.</param>
        /// <param name="value">The value to search for.</param>
        /// <returns>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation,
        /// containing a collection of users, that match the given conditions.
        /// </returns>
        public async Task<IEnumerable<TUser>> FilterAsync(string propertyName, object value)
        {
            return await this.dbContext.FilterAsync(propertyName, value);
        }

        /// <summary>
        /// Finds and returns users, if any, who matches key-value pairs, containing the specified members with the specified values.
        /// </summary>
        /// <param name="matchCollection">
        /// A collection of key-value pairs, 
        /// representing the search member as key and the search term as value.
        /// </param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation,
        /// containing a collection of users, that match the given conditions.
        /// </returns>
        public async Task<IEnumerable<TUser>> FilterAsync(IDictionary<string, object> matchCollection)
        {
            return await this.dbContext.FilterAsync(matchCollection);
        }

        /// <summary>
        /// Finds and returns a user, if any, who has the specified unique id.
        /// </summary>
        /// <param name="id">The user Id to search for.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the user matching the specified userId if it exists.
        /// </returns>
        public async Task<TUser> FindByIdAsync(string id)
        {
            return await this.dbContext.GetAsync(id);
        }

        /// <summary>
        /// Creates (if not exists) or Updates (if exists) the specified user in the user store.
        /// </summary>
        /// <param name="user">The user to be created/updated.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation
        /// </returns>
        public async Task SaveAsync(TUser user)
        {
            await this.dbContext.SaveAsync(user);
        }
    }
}
