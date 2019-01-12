using System.Threading.Tasks;

namespace MSToolKit.Core.EmailServices.Abstraction
{
    /// <summary>
    /// Provides an abstraction for email sending
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Send email to a specified recipient.
        /// </summary>
        /// <param name="formEmailAddress">Sender email address</param>
        /// <param name="fromName">Sender name</param>
        /// <param name="toEmailAddress">Recipient email address</param>
        /// <param name="toName">Recipient name</param>
        /// <param name="title">Message title</param>
        /// <param name="htmlMessage">Message content</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation.
        /// </returns>
        Task SendEmailAsync(
            string formEmailAddress,
            string fromName,
            string toEmailAddress,
            string toName,
            string title,
            string htmlMessage);
    }
}
