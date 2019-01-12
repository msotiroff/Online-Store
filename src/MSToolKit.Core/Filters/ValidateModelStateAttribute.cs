using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Linq;

namespace MSToolKit.Core.Filters
{
    /// <summary>
    /// Validates the ModelState and, if there are any errors, redirects to the GET method with its arguments or,
    /// if does not find it, returns BadRequestResult.
    /// </summary>
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;
            if (!modelState.IsValid)
            {
                var routeValues = context.ActionDescriptor.RouteValues;
                routeValues.TryGetValue("action", out string actionName);
                routeValues.TryGetValue("controller", out string controllerName);
                routeValues.TryGetValue("area", out string areaName);

                var actionArguments = context.ActionArguments.Values;

                var model = actionArguments.FirstOrDefault(aa => !aa.GetType().IsPrimitive && aa.GetType() != typeof(string));
                var modelProperties = model?.GetType().GetProperties();

                var getMethod = context.Controller
                    .GetType()
                    .GetMethods()
                    .FirstOrDefault(mi => mi.Name == actionName
                        && (mi.CustomAttributes
                            .Any(attr => attr.GetType().Name == nameof(HttpGetAttribute))
                                || mi.CustomAttributes.All(attr => !typeof(HttpMethodAttribute).IsAssignableFrom(attr.GetType()))));

                var getMethodArguments = getMethod?
                    .GetParameters()
                    .ToDictionary(
                        pi => pi.Name,
                        pi => modelProperties
                        ?.FirstOrDefault(prop => prop.Name.ToLower() == pi.Name.ToLower())
                            ?.GetValue(model));

                getMethodArguments.Add("area", areaName);

                var result = (actionName == null || controllerName == null || getMethodArguments == null)
                    ? new BadRequestObjectResult(modelState) as IActionResult
                    : new RedirectToActionResult(actionName, controllerName, getMethodArguments) as IActionResult;
                
                context.Result = result;
            }
        }
    }
}
