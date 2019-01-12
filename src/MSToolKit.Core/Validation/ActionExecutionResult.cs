using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSToolKit.Core.Validation
{
    /// <summary>
    /// A class, that contains members, representing result of an action.
    /// </summary>
    public class ActionExecutionResult
    {
        private readonly ICollection<string> errors;

        /// <summary>
        /// Initializes a new instance of MSToolKit.Core.Validation.ActionExecutionResult.
        /// </summary>
        public ActionExecutionResult()
        {
            this.errors = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of MSToolKit.Core.Validation.ActionExecutionResult.
        /// </summary>
        /// <param name="errors">
        /// System.Collections.Generic.IEnumerable of the error messages that should be saved to the current result.
        /// </param>
        public ActionExecutionResult(IEnumerable<string> errors)
        {
            this.errors = errors.ToList();
        }

        /// <summary>
        /// Gets the member that indicates wether the current result is succeeded or not.
        /// </summary>
        public bool Success => !this.errors.Any();

        /// <summary>
        /// Adds an error message to the current result.
        /// </summary>
        /// <param name="errorMessage">
        /// The error message that shold be added.
        /// </param>
        public void AddError(string errorMessage)
        {
            this.errors.Add(errorMessage);
        }

        /// <summary>
        /// Returns a string representation of all the errors that has been added in the current result.
        /// </summary>
        public override string ToString()
        {
            return new StringBuilder()
                .Append("Errors: ")
                .Append(string.Join(", ", this.errors))
                .ToString();
        }
    }
}
