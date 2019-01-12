using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSToolKit.Core.DataAccess.Abstraction
{
    /// <summary>
    /// Provides an abstraction for generic repository.
    /// </summary>
    /// <typeparam name="TEntity">The entity, that current repository represents.</typeparam>
    /// <typeparam name="TKey">The primary key for the given entity.</typeparam>
    public interface IRepository<TEntity, TKey> where TEntity : IEntity<TKey>
    {
        /// <summary>
        /// Gets the specified entity by primary key.
        /// </summary>
        /// <param name="id">The entity primary key to search for.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the entity matching the specified id if it exists.
        /// </returns>
        Task<TEntity> GetAsync(TKey id);

        /// <summary>
        /// Gets all entities from the specified repository.
        /// </summary>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the retrieved entities.
        /// </returns>
        Task<IQueryable<TEntity>> GetAsync();

        /// <summary>
        /// Adds the specified entity in the backing store, as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation.
        /// </returns>
        Task AddAsync(TEntity entity);

        /// <summary>
        /// Returns a collection of entities, whose mathes the given condition.
        /// </summary>
        /// <param name="propertyName">The exact name of the public property(column name)</param>
        /// <param name="value">The needed value</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation,
        /// containing a collection of entities, that matches the given condition.
        /// </returns>
        Task<IEnumerable<TEntity>> FilterAsync(string propertyName, object value);

        /// <summary>
        /// Returns a collection of entities, whose mathes the given conditions.
        /// </summary>
        /// <param name="matchCollection">
        /// Dictionary that keeps the property's names as keys and the needed value as value.
        /// </param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation,
        /// containing a collection of entities, that matches the given conditions.
        /// </returns>
        Task<IEnumerable<TEntity>> FilterAsync(IDictionary<string, object> matchCollection);

        /// <summary>
        /// Removes the specified entity from the backing store, as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The entity to be removed.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation.
        /// </returns>
        Task RemoveAsync(TEntity entity);

        /// <summary>
        /// Updates the specified entity in the backing store, as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation.
        /// </returns>
        Task UpdateAsync(TEntity entity);
    }
}
