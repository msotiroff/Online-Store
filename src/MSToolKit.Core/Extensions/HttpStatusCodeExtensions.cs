using System.Net;

namespace MSToolKit.Core.Extensions
{
    /// <summary>
    /// Provides extension metods to use over HttpStatusCodes.
    /// </summary>
    public static class HttpStatusCodeExtensions
    {
        /// <summary>
        /// Checks if the specified status code is successfull or not.
        /// </summary>
        /// <param name="statusCode">The specified status code.</param>
        /// <returns>True or false, depending of the invokation result.</returns>
        public static bool IsSuccessfull(this HttpStatusCode statusCode)
        {
            var isSuccessfull = (int)statusCode >= 200 && (int)statusCode < 300;

            return isSuccessfull;
        }
    }
}
