using MSToolKit.Core.DataAccess.Abstraction;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSToolKit.Core.DataAccess.Wrappers.DynamoDb.Abstraction
{
    /// <summary>
    /// Provides an abstraction for Amazon Dynamo data access.
    /// </summary>
    /// <typeparam name="TEntity">The type encapsulating a database entity.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for the specified entity.</typeparam>
    public interface IDynamoDbWrapper<TEntity, TKey> : IDbContext<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
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
        Task<IEnumerable<TEntity>> QueryAsync(
            object hashKeyValue, string indexName, bool sortAscending = true);
    }
}
