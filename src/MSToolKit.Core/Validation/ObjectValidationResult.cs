using System.Collections.Generic;
using System.Linq;

namespace MSToolKit.Core.Validation
{
    /// <summary>
    /// A class, that contains members, representing validation results of an object.
    /// </summary>
    public class ObjectValidationResult
    {
        /// <summary>
        /// Initialize a new instance of MSToolKit.Core.Validation.ObjectValidationResult.
        /// </summary>
        public ObjectValidationResult()
        {
            this.Errors = new HashSet<string>();
        }

        /// <summary>
        /// Gets the member, indicating wether the validation is successfull or not.
        /// </summary>
        public bool Success => !this.Errors.Any();

        /// <summary>
        /// Gets a member, containing all errors, if any, occured while validating a specified object.
        /// </summary>
        public ISet<string> Errors { get; }

        /// <summary>
        /// Adds an error to the error list of the current instance.
        /// </summary>
        /// <param name="errorMessage">The error message, that should be added to the error list.</param>
        public void AddError(string errorMessage)
        {
            this.Errors.Add(errorMessage);
        }
    }
}
