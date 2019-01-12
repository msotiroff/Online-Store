using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OnlineStore.Serverless.Infrastructure.Options;
using OnlineStore.Services.Interfaces;
using System.Threading.Tasks;

namespace OnlineStore.Serverless.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryService categoryService;

        public CategoryController(
            IOptions<EnvironmentOptions> environmentOptions,
            ICategoryService categoryService) 
            : base(environmentOptions)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await this.categoryService.GetAsync();

            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id, int pageIndex = 1)
        {
            var category = await this.categoryService
                .GetAsync(id, pageIndex, WebConstants.ProductsCountPerPage);

            return View(category);
        }
    }
}