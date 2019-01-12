using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OnlineStore.Serverless.Infrastructure.Options;
using OnlineStore.Serverless.Models;
using System.Diagnostics;

namespace OnlineStore.Serverless.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(
            IOptions<EnvironmentOptions> environmentOptions) 
            : base(environmentOptions)
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("index", "category");
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Error()
        {
            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(model);
        }
    }
}