using MongoDB.Driver;
using MSToolKit.Core.DataAccess.Abstraction;
using MSToolKit.Core.DataAccess.Wrappers.MongoDb.Abstraction;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSToolKit.Core.DataAccess.Wrappers.MongoDb
{
    /// <summary>
    /// Provides a default implementation for MSToolKit.Core.Wrappers.MongoDb.Abstraction.IMongoDbWrapper.
    /// </summary>
    /// <typeparam name="TEntity">The type encapsulating a database entity.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for the specified entity.</typeparam>
    public class MongoDbWrapper<TEntity, TKey> : IMongoDbWrapper<TEntity, TKey> 
        where TEntity : IEntity<TKey>
    {
        private const string FilterDefinitionKeyValuePair = "'{0}':'{1}'";
        private const string FilterDefinitionTemplate = "{{{0}}}";
        
        private readonly IMongoDatabase dataBase;
        private readonly IMongoCollection<TEntity> collection;

        /// <summary>
        /// Initialize a new instance for MSToolKit.Core.Wrappers.MongoDb.MongoDbWrapper.
        /// </summary>
        /// <param name="dataBase">
        /// An inplementation for MongoDB.Driver.IMongoDatabase.
        /// </param>
        /// <param name="tableName">
        /// The name of the table, that contains database entities for the current type of entities.
        /// </param>
        public MongoDbWrapper(IMongoDatabase dataBase, string tableName)
        {
            this.dataBase = dataBase;
            this.collection = dataBase?.GetCollection<TEntity>(tableName);            
        }

        /// <summary>
        /// Gets an entity by the given identifier as an asynchronous operation.
        /// </summary>
        /// <param name="id">The entity identifier to search for.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the entity matching the specified identifier, if it exists.
        /// </returns>
        public async Task<TEntity> GetAsync(TKey id)
        {
            var entity = (await this.collection.FindAsync(e => e.Id.Equals(id)))
                .SingleOrDefault();

            return entity;
        }

        /// <summary>
        /// Deletes the given entity from the database as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The entity to be deleted.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation.
        /// </returns>
        public async Task DeleteAsync(TEntity entity)
        {
            await this.collection
                .DeleteOneAsync(e => e.Id.Equals(entity.Id));
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
            var filterDefinition = string.Format(FilterDefinitionTemplate,
                string.Format(FilterDefinitionKeyValuePair, propertyName, value));
            
            var entities = await this.collection.Find(filterDefinition).ToListAsync();

            return entities;
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
            var wholeFilterDefinition = string.Format(FilterDefinitionTemplate,
                string.Join(", ", matchCollection
                .Select(mc => string.Format(FilterDefinitionKeyValuePair, mc.Key, mc.Value))));

            var entities = await this.collection.Find(wholeFilterDefinition).ToListAsync();

            return entities;
        }

        /// <summary>
        /// Save the given entity to the database as an asynchronous operation. 
        /// If the given entity already exists, the operation will update it.
        /// </summary>
        /// <param name="entity">The entity, that should be stored.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation.
        /// </returns>
        public async Task SaveAsync(TEntity entity)
        {
            var exist = (await this.collection.FindAsync(e => e.Id.Equals(entity.Id))).Any();
            if (exist)
            {
                await this.collection
                    .ReplaceOneAsync(e => e.Id.Equals(entity.Id), entity);

                return;
            }

            await this.collection.InsertOneAsync(entity);
        }

        /// <summary>
        /// Gets all entities from the specified database table.
        /// </summary>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the retrieved entities.
        /// </returns>
        public async Task<IQueryable<TEntity>> GetAsync()
        {
            var allEntities = await this.collection.FindAsync(_ => true);

            return allEntities.ToList().AsQueryable();
        }
    }
}
