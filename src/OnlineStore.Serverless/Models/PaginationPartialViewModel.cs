using Microsoft.AspNetCore.Mvc.Rendering;
using MSToolKit.Core.Collections.Abstraction;
using System.Collections.Generic;
using System.Linq;

namespace OnlineStore.Serverless.Models
{
    public class PaginationPartialViewModel
    {
        public PaginationPartialViewModel(
            IPaginatedList paginatedList,
            ViewContext viewContext)
        {
            this.SearchTerm = viewContext.HttpContext.Request.Query["searchTerm"];
            this.PaginatedList = paginatedList;
            this.RouteData = viewContext
                .RouteData
                .Values
                .ToDictionary(x => x.Key, v => v.Value.ToString());
        }

        public string SearchTerm { get; set; }

        public IPaginatedList PaginatedList { get; }
        
        public IDictionary<string, string> RouteData { get; }
    }
}
