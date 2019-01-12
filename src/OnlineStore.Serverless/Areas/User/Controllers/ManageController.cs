using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MSToolKit.Core.Authentication;
using MSToolKit.Core.Authentication.Abstraction;
using MSToolKit.Core.EmailServices.Abstraction;
using MSToolKit.Core.Extensions;
using MSToolKit.Core.Filters;
using OnlineStore.Common.Notifications;
using OnlineStore.Common.ViewModels.User;
using OnlineStore.Serverless.Controllers;
using OnlineStore.Serverless.Infrastructure.Options;
using OnlineStore.Services.Interfaces;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using static OnlineStore.Serverless.WebConstants;

namespace OnlineStore.Serverless.Areas.User.Controllers
{
    [Area(UserAreaName)]
    public class ManageController : BaseController
    {
        private readonly IUserManager<OnlineStore.Models.User> userManager;
        private readonly ISignInManager<OnlineStore.Models.User> signInManager;
        private readonly IUserService userService;
        private readonly IEmailSender emailSender;

        public ManageController(
            IOptions<EnvironmentOptions> environmentOptions,
            IUserManager<OnlineStore.Models.User> userManager,
            ISignInManager<OnlineStore.Models.User> signInManager,
            IUserService userService,
            IEmailSender emailSender) : base(environmentOptions)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userService = userService;
            this.emailSender = emailSender;
        }

        [Authorize]
        public async Task<IActionResult> Index(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                id = this.User?.GetIdentifier();
            }

            if (this.User.GetIdentifier() != id 
                && !this.User.IsInRole(AuthenticationConstants.AdministratorRoleName))
            {
                return BadRequest("You do not have access to this resource.");
            }

            var model = await this.userService
                .GetByIdAsync(id ?? this.User.GetIdentifier());

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateModelState]
        public async Task<IActionResult> Edit(UserIndexViewModel model)
        {
            var result = await this.userService.UpdateAsync(model);
            if (!result.Success)
            {
                this.ShowNotification(result.ResponseMessage, NotificationType.Error);
                return RedirectToAction("index", "manage", new { area = "user" });
            }

            this.ShowNotification(
                "You have successfully updated the profile.",
                NotificationType.Success);

            return RedirectToAction(nameof(this.Index), new { id = model.Id });
        }

        [HttpGet]
        [Authorize]        
        public async Task<IActionResult> SendVerificationEmail()
        {
            var user = await this.userManager.FindByIdAsync(this.User.GetIdentifier());
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = this.Url.Action(
                nameof(this.ConfirmEmail), "Manage",
                new { userId = user.Id, code },
                protocol: Request.Scheme);

            await emailSender
                .SendEmailAsync(
                    AdministratorEmail,
                    AdministratorName,
                    user.Email,
                    user.FullName,
                    "Confirm your email",
                    $"Please confirm your email by " +
                    $"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            this.ShowNotification(
                "We have sent a confirmation message to your email.",
                NotificationType.Success);

            return RedirectToHome();
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword(string id, string email)
        {
            if (this.User.GetIdentifier() != id || this.User.GetEmail() != email)
            {
                throw new UnauthorizedAccessException(
                    "You cannot change password of an other user.");
            }

            var model = new ChangePasswordViewModel
            {
                Id = id,
                Email = email
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateModelState]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var user = await this.userManager.FindByIdAsync(this.User.GetIdentifier());
            var result = await this.userManager
                .ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!result.Success)
            {
                this.ShowNotification(result.ResponseMessage, NotificationType.Error);
                return View();
            }

            await this.signInManager.SignOutAsync(this.HttpContext);
            this.ShowNotification(
                "You have successfully updated your password. Please sing in with your new password.",
                NotificationType.Success);

            return RedirectToLogin();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToHome();
            }

            var result = await this.userManager.ConfirmEmailAsync(userId, code);
            if (!result.Success)
            {
                throw new InvalidOperationException(
                    $"Error confirming email for user with ID '{userId}':");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ResetPasswordConfirmed(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToHome();
            }

            var result = await this.userManager.ConfirmResetPasswordTokenAsync(userId, code);
            if (!result.Success)
            {
                throw new InvalidOperationException(
                    $"An error occured while trying to reset password for user with ID: '{userId}'.");
            }

            var model = new NewPasswordViewModel { UserId = userId };

            return View(model);
        }

        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> ResetPasswordConfirmed(NewPasswordViewModel model)
        {
            var result = await this.userManager.SetPasswordAsync(model.UserId, model.NewPassword);
            if (!result.Success)
            {
                throw new InvalidOperationException(
                    $"An error occured while trying to reset password for user with ID: '{model.UserId}'.");
            }

            this.ShowNotification(
                "Your password was reset successfully. Now you can sign in with your new password.",
                NotificationType.Success);

            return RedirectToLogin();
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await this.userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                this.ShowNotification("Message sent. Please, check your email.", NotificationType.Info);
                return RedirectToLogin();
            }

            var code = await this.userManager.GenerateResetPasswordConfirmationTokenAsync(user);
            var callbackUrl = this.Url.Action(
                nameof(this.ResetPasswordConfirmed), "Manage",
                new { userId = user.Id, code },
                protocol: Request.Scheme);

            await this.emailSender.SendEmailAsync(
                AdministratorEmail,
                    AdministratorName,
                    user.Email,
                    user.FullName,
                    "Reset password",
                    $"Please, reset your password by " +
                    $"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            this.ShowNotification(
                "To reset your password, please follow the instructions we have sent to your email.",
                NotificationType.Success);

            return RedirectToHome();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = this.User.GetIdentifier();
            var model = await this.userService.GetForDeleteAsync(userId);

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateModelState]
        public async Task<IActionResult> DeleteAccount(UserDeleteViewModel model)
        {
            var user = await this.userManager
                .CheckPasswordAsync(model.Email, model.Password);

            if (user == null)
            {
                this.ShowNotification(NotificationMessages.InvalidCredentials);

                return View(model);
            }

            await this.userService.DeleteAsync(user);
            await this.signInManager.SignOutAsync(this.HttpContext);

            this.ShowNotification(
                "Your account has been deleted premanently!",
                NotificationType.Success);

            return RedirectToHome();
        }
    }
}