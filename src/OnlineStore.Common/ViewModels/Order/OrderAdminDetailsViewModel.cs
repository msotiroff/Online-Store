using OnlineStore.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Common.ViewModels.Order
{
    public class OrderAdminDetailsViewModel
    {
        [Display(Name = "Order date")]
        public DateTime DateTime { get; set; }

        [Display(Name = "Customer name")]
        public string CustomerName { get; set; }

        [Display(Name = "Customer phone")]
        public string CustomerPhoneNumber { get; set; }

        [Display(Name = "Total price")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "State")]
        public OrderState State { get; set; }

        /// <summary>
        /// Key = product name. Value = product count.
        /// </summary>
        [Display(Name = "Products")]
        public Dictionary<string, int> Products { get; set; }
    }
}
