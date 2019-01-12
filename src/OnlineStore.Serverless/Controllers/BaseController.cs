using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MSToolKit.Core.Validation;
using OnlineStore.Common.Notifications;
using OnlineStore.Serverless.Areas.User.Controllers;
using OnlineStore.Serverless.Infrastructure.Options;
using System.Linq;

namespace OnlineStore.Serverless.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly EnvironmentOptions environmentOptions;

        public BaseController(IOptions<EnvironmentOptions> environmentOptions)
        {
            this.environmentOptions = environmentOptions.Value;
        }

        protected void ShowNotification(
            string message, 
            NotificationType notificationType = NotificationType.Error)
        {
            this.TempData[NotificationConstants.NotificationMessageKey] = message;
            this.TempData[NotificationConstants.NotificationTypeKey] = notificationType.ToString();
        }

        protected void ShowNotification(ActionExecutionResult result)
        {
            var message = result.Success ? "Success" : result.ToString();

            this.ShowNotification(
                message, 
                result.Success 
                    ? NotificationType.Success 
                    : NotificationType.Error);
        }

        protected internal void ShowModelStateError()
        {
            var firstOccuredErrorMsg = ModelState
                .Values
                .FirstOrDefault(v => v.Errors.Any())
                ?.Errors
                .FirstOrDefault()
                ?.ErrorMessage;

            if (firstOccuredErrorMsg != null)
            {
                this.ShowNotification(firstOccuredErrorMsg);
            }
        }

        protected IActionResult RedirectToHome()
            => new RedirectResult(this.environmentOptions.ApplicationBaseEndpoint);

        protected IActionResult RedirectToLocalUrl(string url)
        {
            var isLocalUrl = this.Url.IsLocalUrl(url);

            var fullUrl = string.Concat(
                this.environmentOptions.ApplicationBaseEndpoint, 
                isLocalUrl ? url.TrimStart('/') : string.Empty);

            return new RedirectResult(fullUrl);
        }

        protected IActionResult RedirectToLogin() 
            => RedirectToAction(
                nameof(AccountController.Login), 
                "Account", 
                new { area = WebConstants.UserAreaName });


        protected IActionResult RedirectToProfile()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(
                    nameof(ManageController.Index), 
                    "Manage", 
                    new { area = "user" });
            }

            return this.RedirectToHome();
        }
    }
}