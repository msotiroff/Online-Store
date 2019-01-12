using OnlineStore.Common.ViewModels.Product;
using OnlineStore.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Common.ViewModels.Order
{
    public class OrderDetailsViewModel
    {
        [Display(Name = "Order date")]
        public DateTime DateTime { get; set; }

        [Display(Name = "Customer name")]
        public string CustomerName { get; set; }
        
        [Display(Name = "Total price")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "State")]
        public OrderState State { get; set; }

        /// <summary>
        /// Key = product model. Value = product count.
        /// </summary>
        [Display(Name = "Products")]
        public Dictionary<ProductSimpleViewModel, int> Products { get; set; }
    }
}
