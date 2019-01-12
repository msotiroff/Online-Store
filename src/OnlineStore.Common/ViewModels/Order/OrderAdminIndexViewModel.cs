using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineStore.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OnlineStore.Common.ViewModels.Order
{
    public class OrderAdminIndexViewModel
    {
        public OrderAdminIndexViewModel()
        {
            this.AllStates = Enum
                .GetNames(typeof(OrderState))
                .Select(os => new SelectListItem(os, os));
        }

        public string Id { get; set; }

        [Display(Name = "Order date")]
        public DateTime DateTime { get; set; }

        [Display(Name = "Customer name")]
        public string CustomerName { get; set; }

        [Display(Name = "Customer phone")]
        public string CustomerPhoneNumber { get; set; }
        
        [Display(Name = "Total price")]
        public decimal TotalAmount { get; set; }
        
        public OrderState State { get; set; }

        [Display(Name = "Order state")]
        public IEnumerable<SelectListItem> AllStates { get; set; }
    }
}
