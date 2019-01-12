namespace MSToolKit.Core.Filters.Options
{
    /// <summary>
    /// Specifies options for LoggingExceptionHandler requirements.
    /// </summary>
    public class LoggingExceptionHandlerOptions
    {
        /// <summary>
        /// Gets or sets the relative path to the local error url.
        /// </summary>
        public string LocalErrorUrl { get; set; }

        /// <summary>
        /// Gets or sets a member, that indicates wether the original exception should be rethrown.
        /// </summary>
        public bool ShouldRethrowException { get; set; } = true;
    }
}
