using System;

namespace MSToolKit.Core.DataAccess.Abstraction
{
    /// <summary>
    /// Provides an abstraction for a database entity.
    /// </summary>
    /// <typeparam name="TKey">
    /// The primary key for the given entity.
    /// </typeparam>
    public interface IEntity<TKey>
    {
        /// <summary>
        /// Gets or sets the primary key for the specified entity.
        /// </summary>
        TKey Id { get; set; }
    }
}
