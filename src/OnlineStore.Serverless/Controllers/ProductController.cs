using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OnlineStore.Serverless.Infrastructure.Options;
using OnlineStore.Services.Interfaces;
using System.Threading.Tasks;

namespace OnlineStore.Serverless.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService productService;

        public ProductController(
            IOptions<EnvironmentOptions> environmentOptions,
            IProductService productService) 
            : base(environmentOptions)
        {
            this.productService = productService;
        }

        public async Task<IActionResult> Details(string id)
        {
            var model = await this.productService.GetAsync(id);

            return View(model);
        }
    }
}