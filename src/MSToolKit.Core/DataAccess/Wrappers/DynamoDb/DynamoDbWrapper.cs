using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using MSToolKit.Core.DataAccess.Abstraction;
using MSToolKit.Core.DataAccess.Wrappers.DynamoDb.Abstraction;
using MSToolKit.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSToolKit.Core.DataAccess.Wrappers.DynamoDb
{
    /// <summary>
    /// Provides a default implementation for MSToolKit.Core.Wrappers.DynamoDb.Abstraction.IDynamoDbWrapper.
    /// </summary>
    /// <typeparam name="TEntity">The type encapsulating a database entity.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for the specified entity.</typeparam>
    public class DynamoDbWrapper<TEntity, TKey> : DynamoDBContext, IDynamoDbWrapper<TEntity, TKey> 
        where TEntity : IEntity<TKey>
    {
        private readonly DynamoDBOperationConfig config;

        /// <summary>
        /// Initialize a new nistance of MSToolKit.Core.Wrappers.DynamoDb.DynamoDbWrapper.
        /// </summary>
        /// <param name="client">
        /// An instance for Amazon.DynamoDBv2.IAmazonDynamoDB.
        /// </param>
        /// <param name="tableName">
        /// The name of the table, that contains database entities for the current type of entities.
        /// </param>
        public DynamoDbWrapper(IAmazonDynamoDB client, string tableName) 
            : base(client)
        {
            this.config = new DynamoDBOperationConfig()
            {
                OverrideTableName = tableName 
                    ?? throw new ArgumentNullException(nameof(tableName))
            };
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
            return await base.LoadAsync<TEntity>(id, this.config);
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
            await base.DeleteAsync(entity, this.config);
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
            if (!typeof(TEntity).HasPropertyOfType(value.GetType(), propertyName))
            {
                return null;
            }

            var conditions = new List<ScanCondition>
            {
                new ScanCondition(propertyName, ScanOperator.Equal, value)
            };

            try
            {
                var entities = await base
                .ScanAsync<TEntity>(conditions, this.config)
                .GetRemainingAsync();

                return entities;
            }
            catch (InvalidOperationException)
            {
                return new List<TEntity>();
            }
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
            var conditions = new List<ScanCondition>();

            foreach (var item in matchCollection)
            {
                var propertyName = item.Key;
                var propertyValue = item.Value;

                if (!typeof(TEntity).HasPropertyOfType(propertyValue.GetType(), propertyName))
                {
                    continue;
                }

                conditions.Add(new ScanCondition(propertyName, ScanOperator.Equal, propertyValue));
            }

            try
            {
                var entities = await base
                .ScanAsync<TEntity>(conditions, this.config)
                .GetRemainingAsync();

                return entities;
            }
            catch (InvalidOperationException)
            {
                return new List<TEntity>();
            }
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
            await base.SaveAsync(entity, this.config);
        }

        /// <summary>
        /// Perform an asychronous query over the current instance of IDynamoDbWrapper
        /// and returns a collection of entities, that match the given hashkey value.
        /// </summary>
        /// <param name="hashKeyValue">The value of the hash key, that should match in the query.</param>
        /// <param name="indexName">The name of the index, that should be used in this query.</param>
        /// <param name="sortAscending">
        /// An argument, that indicates wether the result set should be ordered ascending or not. 
        /// If 'true', then the result set will be sorted in ascending order, 
        /// otherwise will be sorted in descending order by the sort key for that index. 
        /// Default value: true.
        /// </param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing a collection of entities, that match the given hashkey value.
        /// </returns>
        public async Task<IEnumerable<TEntity>> QueryAsync(
            object hashKeyValue, string indexName, bool sortAscending = true)
        {
            if (string.IsNullOrWhiteSpace(indexName))
            {
                throw new ArgumentNullException($"{nameof(indexName)} cannot be empty!");
            }

            this.config.IndexName = indexName;
            this.config.BackwardQuery = !sortAscending;

            var entities = await base
                .QueryAsync<TEntity>(hashKeyValue, this.config)
                .GetRemainingAsync();

            return entities;
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
            var allEntities = await base
                .ScanAsync<TEntity>(new List<ScanCondition>(), this.config)
                .GetRemainingAsync();

            return allEntities.AsQueryable();
        }
    }
}
