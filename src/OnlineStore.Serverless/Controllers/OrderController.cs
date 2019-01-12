using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MSToolKit.Core.Extensions;
using MSToolKit.Core.Filters;
using OnlineStore.Common.Notifications;
using OnlineStore.Common.ViewModels.Order;
using OnlineStore.Serverless.Infrastructure.Extensions;
using OnlineStore.Serverless.Infrastructure.Options;
using OnlineStore.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Serverless.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IShoppingCartService shoppingCartService;
        private readonly IOrderService orderService;

        public OrderController(
            IOptions<EnvironmentOptions> environmentOptions,
            IShoppingCartService shoppingCartService,
            IOrderService orderService) 
            : base(environmentOptions)
        {
            this.shoppingCartService = shoppingCartService;
            this.orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var browserId = this.HttpContext.GetBrowserId();
            var shoppingCart = await this.shoppingCartService.GetAsync(browserId);

            var model = new OrderCreateViewModel
            {
                ShoppingCart = shoppingCart
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateViewModel model)
        {
            var browserId = this.HttpContext.GetBrowserId();
            model.BrowserId = browserId;
            model.ShoppingCart = await this.shoppingCartService.GetAsync(browserId);
            model.UserId = this.User.Identity.IsAuthenticated
                ? this.User.GetIdentifier()
                : string.Empty;

            if (model.ShoppingCart.Items.Any(i => i.Count > i.Product.Availability))
            {
                this.ShowNotification(
                    $"Insufficient availability of some products, " +
                    $"please go to your Shopping cart for more information.");

                return View();
            }

            await this.orderService.CreateAsync(model);
            await this.shoppingCartService.RemoveAsync(browserId);
            this.HttpContext.ClearBrowserId();

            this.ShowNotification(
                "Thank you for your order. We will contact you soon.",
                NotificationType.Success);

            return RedirectToHome();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> History(int pageIndex = 1)
        {
            var userId = this.User.GetIdentifier();
            var orders = await this.orderService
                .GetOrdersByUserIdAsync(userId, pageIndex, WebConstants.OrdersCountPerPage);

            return View(orders);
        }

        [HttpGet]
        [Authorize]
        [ValidateQueryParameters]
        public async Task<IActionResult> Details(string id)
        {
            var order = await this.orderService.GetAsync(id);

            return View(order);
        }
    }
}