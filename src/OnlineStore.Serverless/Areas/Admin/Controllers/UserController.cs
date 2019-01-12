using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MSToolKit.Core.Authentication.Abstraction;
using MSToolKit.Core.Extensions;
using OnlineStore.Common.Notifications;
using OnlineStore.Serverless.Infrastructure.Options;
using OnlineStore.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Serverless.Areas.Admin.Controllers
{
    public class UserController : BaseAdminController
    {
        private readonly IUserManager<OnlineStore.Models.User> userManager;
        private readonly IUserService userService;

        public UserController(
            IOptions<EnvironmentOptions> environmentOptions,
            IUserManager<OnlineStore.Models.User> userManager,
            IUserService userService) : base(environmentOptions)
        {
            this.userManager = userManager;
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageIndex = 1, string searchTerm = null)
        {
            searchTerm = !string.IsNullOrWhiteSpace(searchTerm) ? searchTerm : "";

            var users = (await this.userService.GetAsync())
                .Where(u => u.Email.ToLower().Contains(searchTerm))
                .OrderByMember(nameof(OnlineStore.Models.User.Email))
                .ToPaginatedList(pageIndex, WebConstants.UsersCountPerPage);

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> AddToRoleAdmin(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);
            var result = await this.userManager.AddToRoleAdminAsync(user);

            this.ShowNotification(
                result.ResponseMessage, 
                result.Success 
                    ? NotificationType.Success 
                    : NotificationType.Error);

            return RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        public async Task<IActionResult> RemoveFromRoleAdmin(string id)
        {
            if (this.User.GetIdentifier() == id)
            {
                this.ShowNotification("Cannot remove yourself from role Admin.", NotificationType.Warning);
                return RedirectToAction(nameof(this.Index));
            }

            var user = await this.userManager.FindByIdAsync(id);
            var result = await this.userManager.RemoveFromRoleAdminAsync(user);

            this.ShowNotification(
                result.ResponseMessage,
                result.Success
                    ? NotificationType.Success
                    : NotificationType.Error);

            return RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (this.User.GetIdentifier() == id)
            {
                this.ShowNotification("Cannot delete yourself!", NotificationType.Warning);
                return RedirectToAction(nameof(this.Index));
            }

            var user = await this.userManager.FindByIdAsync(id);
            var result = await this.userManager.DeleteAsync(user);

            this.ShowNotification(
                result.ResponseMessage,
                result.Success
                    ? NotificationType.Success
                    : NotificationType.Error);

            return RedirectToAction(nameof(this.Index));
        }
    }
}