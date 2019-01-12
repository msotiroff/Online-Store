using MSToolKit.Core.DataAccess.Abstraction;

namespace MSToolKit.Core.DataAccess.Wrappers.MongoDb.Abstraction
{
    /// <summary>
    /// Provides an abstraction for MongoDb data access.
    /// </summary>
    /// <typeparam name="TEntity">The type encapsulating a database entity.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for the specified entity.</typeparam>
    public interface IMongoDbWrapper<TEntity, TKey> : IDbContext<TEntity, TKey> 
        where TEntity : IEntity<TKey>
    {
    }
}
