using System.ComponentModel.DataAnnotations;

namespace MSToolKit.Core.DataAccess.Wrappers.DynamoDb
{
    /// <summary>
    /// Specifies options for DynamoDB table requirements
    /// </summary>
    /// <typeparam name="TEntity">The type encapsulating an entity.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for the given entity.</typeparam>
    public class DynamoDbTableOptions<TEntity, TKey>
    {
        /// <summary>
        /// The name of the table, that contains the given entity type.
        /// </summary>
        [Required]
        public string TableName { get; set; }
    }
}
