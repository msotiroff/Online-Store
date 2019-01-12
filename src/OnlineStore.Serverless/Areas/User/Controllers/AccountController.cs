using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MSToolKit.Core.Authentication.Abstraction;
using MSToolKit.Core.EmailServices.Abstraction;
using MSToolKit.Core.Filters;
using OnlineStore.Common.Notifications;
using OnlineStore.Common.ViewModels.User;
using OnlineStore.Serverless.Controllers;
using OnlineStore.Serverless.Infrastructure.Options;
using OnlineStore.Services.Interfaces;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

using static OnlineStore.Serverless.WebConstants;

namespace OnlineStore.Serverless.Areas.User.Controllers
{
    [Area(UserAreaName)]
    public class AccountController : BaseController
    {
        private readonly IUserManager<OnlineStore.Models.User> userManager;
        private readonly ISignInManager<OnlineStore.Models.User> signInManager;
        private readonly IEmailSender emailSender;
        private readonly IUserService userService;

        public AccountController(
            IOptions<EnvironmentOptions> environmentOptions,
            IUserManager<OnlineStore.Models.User> userManager,
            ISignInManager<OnlineStore.Models.User> signInManager,
            IEmailSender emailSender,
            IUserService userService) : base(environmentOptions)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                this.ShowNotification("You are already logged in!", NotificationType.Info);
                this.RedirectToHome();
            }

            return View();
        }

        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await this.signInManager
                .SignInAsync(
                    this.ControllerContext.HttpContext,
                    model.Email,
                    model.Password,
                    model.RememberMe);

            if (result.Success)
            {
                this.ShowNotification(result.ResponseMessage, NotificationType.Success);

                return this.RedirectToHome();
            }

            var user = await this.userManager.FindByEmailAsync(model.Email);

            if (this.signInManager.RequireConfirmedEmail && user != null && !user.EmailConfirmed)
            {
                await this.SendVerificationEmailAsync(user);
            }

            this.ShowNotification(result.ResponseMessage, NotificationType.Error);

            return this.RedirectToHome();
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            await this.signInManager
                .SignOutAsync(this.ControllerContext.HttpContext);

            return View();
        }

        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var passwordValidationResult = this.userManager.ValidatePassword(model.Password);
            if (!passwordValidationResult.Success)
            {
                this.ShowNotification(passwordValidationResult.ResponseMessage);
                return View();
            }

            var passwordHash = this.userManager.GetPasswordHash(model.Password);
            var user = new OnlineStore.Models.User(
                model.Email, passwordHash, model.FirstName, model.LastName);
            var registrationResult = await this.userManager.CreateAsync(user);
            if (!registrationResult.Success)
            {
                this.ShowNotification(registrationResult.ResponseMessage);
                return View();
            }

            if (!this.signInManager.RequireConfirmedEmail)
            {
                return RedirectToLogin();
            }

            await this.SendVerificationEmailAsync(user);

            this.ShowNotification(SuccessfullRegistrationEmailTitle, NotificationType.Success);
            return RedirectToLogin();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SignOut()
        {
            await this.signInManager
                .SignOutAsync(this.ControllerContext.HttpContext);

            return this.RedirectToHome();
        }

        private async Task SendVerificationEmailAsync(OnlineStore.Models.User user)
        {
            /// Send confirmation email:
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = this.Url.Action(
                "ConfirmEmail", "Manage",
                new { userId = user.Id, code = code },
                protocol: Request.Scheme);

            await this.emailSender
                .SendEmailAsync(
                    AdministratorEmail,
                    AdministratorName,
                    user.Email,
                    user.FullName,
                    "Confirm your email",
                    $"Please confirm your account by " +
                    $"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
        }
    }
}