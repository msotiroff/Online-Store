using MSToolKit.Core.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSToolKit.Core.DataAccess.Abstraction
{
    /// <summary>
    /// The base implementation of IRepository.
    /// </summary>
    /// <typeparam name="TEntity">The entity, that current repository represents.</typeparam>
    /// <typeparam name="TKey">The primary key for the given entity.</typeparam>
    public abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        protected readonly IDbContext<TEntity, TKey> dbContext;

        /// <summary>
        /// Initializes a new instance of MSToolKit.Core.DataAccess.Abstraction.Repository.
        /// </summary>
        /// <param name="dbContext">
        /// Represents a session with the database and can be used to query and save instances of your entities.
        /// </param>
        protected Repository(IDbContext<TEntity, TKey> dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Adds the specified entity in the backing store, as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation.
        /// </returns>
        public async Task AddAsync(TEntity entity)
        {
            CoreValidator.ThrowIfInvalidState(entity);

            await this.dbContext.SaveAsync(entity);
        }

        /// <summary>
        /// Gets all entities from the specified repository.
        /// </summary>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the retrieved entities.
        /// </returns>
        public async Task<IQueryable<TEntity>> GetAsync()
        {
            return await this.dbContext.GetAsync();
        }

        /// <summary>
        /// Gets the specified entity by primary key.
        /// </summary>
        /// <param name="id">The entity primary key to search for.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the entity matching the specified id if it exists.
        /// </returns>
        public async Task<TEntity> GetAsync(TKey id)
        {
            return await this.dbContext.GetAsync(id);
        }

        /// <summary>
        /// Returns a collection of entities, whose mathes the given condition.
        /// </summary>
        /// <param name="propertyName">The exact name of the public property(column name)</param>
        /// <param name="value">The needed value</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation,
        /// containing a collection of entities, that matches the given condition.
        /// </returns>
        public async Task<IEnumerable<TEntity>> FilterAsync(string propertyName, object value)
        {
            return await this.dbContext.FilterAsync(propertyName, value);
        }

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
        public async Task<IEnumerable<TEntity>> FilterAsync(IDictionary<string, object> matchCollection)
        {
            return await this.dbContext.FilterAsync(matchCollection);
        }

        /// <summary>
        /// Removes the specified entity from the backing store, as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The entity to be removed.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation.
        /// </returns>
        public async Task RemoveAsync(TEntity entity)
        {
            await this.dbContext.DeleteAsync(entity);
        }

        /// <summary>
        /// Updates the specified entity in the backing store, as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation.
        /// </returns>
        public async Task UpdateAsync(TEntity entity)
        {
            CoreValidator.ThrowIfInvalidState(entity);

            var exist = await this.GetAsync(entity.Id) != null;
            if (!exist)
            {
                throw new InvalidOperationException(
                    $"{entity.GetType().Name} does not exist!");
            }

            await this.dbContext.SaveAsync(entity);
        }
    }
}
