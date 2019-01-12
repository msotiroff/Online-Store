using Amazon;

namespace MSToolKit.Core.DataAccess.Wrappers.DynamoDb
{
    /// <summary>
    /// Specifies options for DynamoDB requirements.
    /// </summary>
    public class DynamoDbOptions
    {
        /// <summary>
        /// Gets or sets the AccessKeyId for Amazon.DynamoDBv2.IAmazonDynamoDB.
        /// </summary>
        public string AccessKeyId { get; set; }

        /// <summary>
        /// Gets or sets the SecretAccessKey for Amazon.DynamoDBv2.IAmazonDynamoDB.
        /// </summary>
        public string SecretAccessKey { get; set; }

        /// <summary>
        /// Gets or sets a member, that indicates wether the local mode for dynamo database should be used.
        /// </summary>
        public bool LocalMode { get; set; }

        /// <summary>
        /// Gets or sets the service url, that should be used. Required for Local mode.
        /// </summary>
        public string ServiceUrl { get; set; }

        /// <summary>
        /// Gets or sets the region endpoint, that should be used.
        /// </summary>
        public RegionEndpoint RegionEndpoint { get; set; }
    }
}
