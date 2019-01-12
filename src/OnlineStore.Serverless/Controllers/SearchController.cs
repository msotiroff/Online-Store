using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OnlineStore.Serverless.Infrastructure.Options;
using OnlineStore.Services.Interfaces;
using System.Threading.Tasks;

namespace OnlineStore.Serverless.Controllers
{
    public class SearchController : BaseController
    {
        private readonly IProductService productService;

        public SearchController(
            IOptions<EnvironmentOptions> environmentOptions,
            IProductService productService) 
            : base(environmentOptions)
        {
            this.productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Results(int pageIndex = 1, string searchTerm = null)
        {
            var results = await this.productService
                .FilterAsync(searchTerm, pageIndex, WebConstants.SearchResultItemsCountPerPage);

            return View(results);
        }
    }
}