namespace MSToolKit.Core.Authentication
{
    /// <summary>
    /// Represents the result of an authentication operation.
    /// </summary>
    public class AuthenticationResult
    {
        /// <summary>
        /// Initializes a new instance of MSToolKit.Core.Authentication.AuthenticationResult.
        /// </summary>
        /// <param name="success">Wether the result is successfull or not.</param>
        /// <param name="responseMessage">The response message for the instance.</param>
        public AuthenticationResult(bool success, string responseMessage = default(string))
        {
            this.Success = success;
            this.ResponseMessage = responseMessage;
        }

        /// <summary>
        /// Member indicating whether if the operation succeeded or not.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Member that contains a message that describes the result of the authentication operation.
        /// </summary>
        public string ResponseMessage { get; }
    }
}
