using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MSToolKit.Core.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace MSToolKit.Core.Filters
{
    /// <summary>
    /// Validates that the query parameters, required for current action are not null, 
    /// if there are any missing parameters returns BadRequestResult.
    /// Optionally can be passed just part of the action arguments that should be validated. 
    /// If no arguments specified - all the action arguments will be validated.
    /// </summary>
    public class ValidateQueryParametersAttribute : ActionFilterAttribute
    {
        private IEnumerable<string> queryParametersNames;

        /// <summary>
        /// Initializes a new instance of MSToolKit.Core.Filters.ValidateQueryParametersAttribute.
        /// </summary>
        /// <param name="actionArgumentsToValidate">
        /// Action arguments that should be validated.
        /// </param>
        public ValidateQueryParametersAttribute(params string[] actionArgumentsToValidate)
        {
            this.queryParametersNames = actionArgumentsToValidate;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var actionArguments = context.ActionArguments;
            if (this.queryParametersNames.IsNullOrEmpty())
            {
                this.queryParametersNames = context
                    .ActionDescriptor
                    .Parameters
                    .Select(pi => pi.Name);
            }

            foreach (var parameter in queryParametersNames)
            {
                actionArguments.TryGetValue(parameter, out object queryValue);

                if (queryValue == null)
                {
                    context.Result = new BadRequestResult();
                }
            }
        }
    }
}
