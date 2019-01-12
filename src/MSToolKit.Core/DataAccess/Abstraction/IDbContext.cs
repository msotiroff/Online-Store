using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSToolKit.Core.DataAccess.Abstraction
{
    /// <summary>
    /// Provides an abstraction for database access.
    /// </summary>
    /// <typeparam name="TEntity">The entity, that current dbContext represents.</typeparam>
    /// <typeparam name="TKey">The primary key for the specified entity.</typeparam>
    public interface IDbContext<TEntity, TKey> where TEntity : IEntity<TKey>
    {
        /// <summary>
        /// Deletes the given entity from the database.
        /// </summary>
        /// <param name="entity">The entity to be deleted.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation.
        /// </returns>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Returns a single entity by the given identifier.
        /// </summary>
        /// <param name="id">The identifier to search for.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation,
        /// containing the entity, if exists, that has the given identifier.
        /// </returns>
        Task<TEntity> GetAsync(TKey id);

        /// <summary>
        /// Gets all entities from the specified database table.
        /// </summary>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the retrieved entities.
        /// </returns>
        Task<IQueryable<TEntity>> GetAsync();

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
        /// If the entity exists - updates it, if not - creates it.
        /// </summary>
        /// <param name="entity">The entity, that should be created/updated.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation.
        /// </returns>
        Task SaveAsync(TEntity entity);
    }
}