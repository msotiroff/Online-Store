using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MSToolKit.Core.Filters;
using OnlineStore.Serverless.Controllers;
using OnlineStore.Serverless.Infrastructure.Options;

namespace OnlineStore.Serverless.Areas.Admin.Controllers
{
    [Authorize]
    [AdministratorsOnly]
    [Area(WebConstants.AdminAreaName)]
    public class BaseAdminController : BaseController
    {
        public BaseAdminController(IOptions<EnvironmentOptions> environmentOptions) 
            : base(environmentOptions)
        {
        }
    }
}