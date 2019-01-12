using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSToolKit.Core.Authentication.Abstraction
{
    /// <summary>
    /// Provides an abstraction for a store which manages user accounts.
    /// </summary>
    /// <typeparam name="TUser">
    /// The type encapsulating a user.
    /// </typeparam>
    public interface IUserStore<TUser> where TUser : AuthenticationUser
    {
        /// <summary>
        /// Deletes the specified user from the user store.
        /// </summary>
        /// <param name="user">The user to be deleted.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation.
        /// </returns>
        Task DeleteAsync(TUser user);

        /// <summary>
        /// Finds and returns a user, if any, who has the specified unique id.
        /// </summary>
        /// <param name="id">The user Id to search for.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the user matching the specified userId if it exists.
        /// </returns>
        Task<TUser> FindByIdAsync(string id);

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
        Task<IEnumerable<TUser>> FilterAsync(string propertyName, object value);

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
        Task<IEnumerable<TUser>> FilterAsync(IDictionary<string, object> matchCollection);

        /// <summary>
        /// Creates (if not exists) or Updates (if exists) the specified user in the user store.
        /// </summary>
        /// <param name="user">The user to be created/updated.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation
        /// </returns>
        Task SaveAsync(TUser user);
    }
}
