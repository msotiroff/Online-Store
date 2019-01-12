using System.ComponentModel.DataAnnotations;

namespace MSToolKit.Core.DataAccess.Wrappers.MongoDb
{
    /// <summary>
    /// Specifies options for MongoDb requirements.
    /// </summary>
    public class MongoDbOptions
    {
        /// <summary>
        /// Gets or sets the name of the database, that should be used.
        /// </summary>
        [Required]
        public string DatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the connection string, that should be used.
        /// </summary>
        [Required]
        public string ConnectionString { get; set; }
    }
}
