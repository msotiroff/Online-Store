using Amazon.S3;
using System.IO;
using System.Threading.Tasks;

namespace MSToolKit.Core.IO.AmazonFileManagment.Abstraction
{
    /// <summary>
    /// Provides an abstraction for CRUD operations over Amazon S3 bucket files.
    /// </summary>
    public interface IAmazonS3FileManager
    {
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
        Task<byte[]> GetFileBytesAsync(string bucketName, string keyName);

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
        Task<string> UploadFileAsync(
            string bucketName, Stream fileStream, string keyName, S3CannedACL s3CannedACL);

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
        Task RemoveFileByUrlAsync(string bucketName, string fileUrl);

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
        Task RemoveFileAsync(string bucketName, string keyName);
    }
}