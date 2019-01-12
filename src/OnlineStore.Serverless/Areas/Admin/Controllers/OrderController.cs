using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MSToolKit.Core.Filters;
using OnlineStore.Common.ViewModels.Enums;
using OnlineStore.Models;
using OnlineStore.Serverless.Infrastructure.Options;
using OnlineStore.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace OnlineStore.Serverless.Areas.Admin.Controllers
{
    public class OrderController : BaseAdminController
    {
        private static string currentOrderMember = nameof(Order.State);
        private static string currentSortDirection = nameof(SortDirection.Ascending);

        private readonly IOrderService orderService;
        
        public OrderController(
            IOptions<EnvironmentOptions> environmentOptions, 
            IOrderService orderService) : base(environmentOptions)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            int pageIndex = 1,
            string orderMember = null, 
            string sortDirection = null)
        {

            orderMember = orderMember ?? currentOrderMember;
            sortDirection = sortDirection ?? currentSortDirection;

            currentOrderMember = orderMember;
            currentSortDirection = sortDirection;

            this.ViewData["orderMember"] = orderMember;
            this.ViewData["sortDirection"] = sortDirection;

            var sortDirEnumeration = Enum.Parse<SortDirection>(sortDirection);
            
            var model = await this.orderService.GetAsync(
                pageIndex, WebConstants.OrdersCountPerPage, orderMember, sortDirEnumeration);

            return View(model);
        }

        [HttpGet]
        [ValidateQueryParameters]
        public async Task<IActionResult> Details(string id)
        {
            var model = await this.orderService.GetForAdminAsync(id);

            return View(model);
        }

        [HttpPost]
        [ValidateQueryParameters]
        public async Task<IActionResult> UpdateState(string orderId, string newState)
        {
            await this.orderService.UpodateStateAsync(orderId, newState);

            return RedirectToAction(nameof(this.Index));
        }
    }
}