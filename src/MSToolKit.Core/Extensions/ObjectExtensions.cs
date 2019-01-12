using MSToolKit.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MSToolKit.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for the base Object class.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Validates wether the specified object is in a valid state, depending on its attributes.
        /// </summary>
        /// <param name="obj">The object to be validated.</param>
        /// <returns>New instance of MSToolKit.Core.Validation.ObjectValidationResult.</returns>
        public static ObjectValidationResult GetValidationResult(this object obj)
        {
            var result = new ObjectValidationResult();

            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);

            if (!isValid)
            {
                foreach (var validationResult in validationResults)
                {
                    result.AddError(validationResult.ErrorMessage);
                }
            }

            return result;
        }
    }
}
