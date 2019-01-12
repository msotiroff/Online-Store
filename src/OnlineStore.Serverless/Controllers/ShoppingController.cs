using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OnlineStore.Common.Notifications;
using OnlineStore.Serverless.Infrastructure.Extensions;
using OnlineStore.Serverless.Infrastructure.Options;
using OnlineStore.Services.Interfaces;
using System.Threading.Tasks;

namespace OnlineStore.Serverless.Controllers
{
    public class ShoppingController : BaseController
    {
        private readonly IShoppingCartService shoppingCartService;

        public ShoppingController(
            IOptions<EnvironmentOptions> environmentOptions,
            IShoppingCartService shoppingCartService) 
            : base(environmentOptions)
        {
            this.shoppingCartService = shoppingCartService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var browserId = this.HttpContext.GetBrowserId();
            var shoppingCart = await this.shoppingCartService.GetAsync(browserId);

            return View(shoppingCart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(string productId, string returnUrl)
        {
            var browserId = this.HttpContext.GetBrowserId();
            await this.shoppingCartService.AddProductAsync(browserId, productId, 1);

            this.ShowNotification(
                "Product added successully to shopping cart.",
                NotificationType.Success);

            return this.RedirectToLocalUrl(returnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> SubtractFromCart(string productId)
        {
            var browserId = this.HttpContext.GetBrowserId();
            await this.shoppingCartService.SubtractProductAsync(browserId, productId, 1);

            return this.RedirectToCart();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(string productId)
        {
            var browserId = this.HttpContext.GetBrowserId();
            await this.shoppingCartService.RemoveProductAsync(browserId, productId);

            return this.RedirectToCart();
        }
        
        private IActionResult RedirectToCart()
        {
            return RedirectToAction(nameof(this.Index));
        }
    }
}