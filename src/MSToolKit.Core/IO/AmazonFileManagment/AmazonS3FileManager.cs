using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Logging;
using MSToolKit.Core.IO.AmazonFileManagment.Abstraction;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MSToolKit.Core.IO.AmazonFileManagment
{
    internal class AmazonS3FileManager : IAmazonS3FileManager
    {
        private readonly IAmazonS3 s3Client;
        private readonly ITransferUtility transferUtility;
        private readonly ILogger<IAmazonS3FileManager> logger;

        /// <summary>
        /// Initializes a new instance of MSToolKit.Core.IO.AmazonFileManagment.AmazonS3FileManager.
        /// </summary>
        public AmazonS3FileManager(
            IAmazonS3 s3Client, 
            ITransferUtility transferUtility,
            ILogger<IAmazonS3FileManager> logger)
        {
            this.s3Client = s3Client;
            this.transferUtility = transferUtility;
            this.logger = logger;
        }

        /// <summary>
        /// Uploads a file to a specified S3 bucket with specified key name.
        /// </summary>
        /// <param name="bucketName">
        /// The bucket that should be the file uploaded to.
        /// </param>
        /// <param name="fileStream">
        /// The stream that contains the file bytes.
        /// </param>
        /// <param name="keyName">
        /// The uploded file key name.
        /// </param>
        /// <param name="s3CannedACL">
        /// Instance of Amazon.S3.S3CannedACL that specifies the uploaded file access.
        /// </param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation,
        /// containing the public url of the uploaded file.
        /// </returns>
        public async Task<string> UploadFileAsync(
            string bucketName, Stream stream, string keyName, S3CannedACL s3CannedACL)
        {
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = keyName,
                BucketName = bucketName,
                CannedACL = s3CannedACL,
            };
            
            await transferUtility.UploadAsync(uploadRequest);

            return $@"https://{bucketName}.s3.amazonaws.com/{keyName}";
        }

        /// <summary>
        /// Returns the specified file bytes.
        /// </summary>
        /// <param name="bucketName">
        /// The bucket that should contain the file specified.
        /// </param>
        /// <param name="keyName">
        /// The file key name.
        /// </param>
        /// <returns>
        /// Byte array, containing the specified file's bytes.
        /// </returns>
        public async Task<byte[]> GetFileBytesAsync(string bucketName, string keyName)
        {
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = keyName
            };

            using (var objectResponse = await this.s3Client.GetObjectAsync(request))
            {
                using (var responseStream = objectResponse.ResponseStream)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        responseStream.CopyTo(memoryStream);
                        return memoryStream.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// Removes a specified file from an Amazon S3 bucket.
        /// </summary>
        /// <param name="bucketName">
        /// The bucket that should be the file removed from.
        /// </param>
        /// <param name="fileUrl">
        /// The url of the file that should be removed.
        /// </param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation.
        /// </returns>
        public async Task RemoveFileByUrlAsync(string bucketName, string fileUrl)
        {
            var keyName = Regex
                .Match(fileUrl, @"\.s3\.amazonaws\.com\/(?<KeyName>.+)$")
                .Groups["KeyName"]
                .Value;

            if (string.IsNullOrWhiteSpace(keyName))
            {
                throw new ArgumentException(
                    $"Could not extract key name from the given url: {fileUrl}.");
            }

            var request = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = keyName
            };

            await this.s3Client.DeleteObjectAsync(request);
        }

        /// <summary>
        /// Removes a specified file from an Amazon S3 bucket.
        /// </summary>
        /// <param name="bucketName">
        /// The bucket that should be the file removed from.
        /// </param>
        /// <param name="keyName">
        /// The url of the file that should be removed.
        /// </param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation.
        /// </returns>
        public async Task RemoveFileAsync(string bucketName, string keyName)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = keyName
            };

            await this.s3Client.DeleteObjectAsync(request);
        }
    }
}
