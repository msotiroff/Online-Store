using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MSToolKit.Core.Authentication;

namespace MSToolKit.Core.Filters
{
    public class AdministratorsOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isCurrentLoggedUserAdmin = context
                .HttpContext
                .User
                .IsInRole(AuthenticationConstants.AdministratorRoleName);

            if (!isCurrentLoggedUserAdmin)
            {
                context.Result = new BadRequestResult();
            }

            base.OnActionExecuting(context);
        }
    }
}
