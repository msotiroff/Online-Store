using OnlineStore.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Common.ViewModels.Order
{
    public class OrderIndexViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Order date")]
        public DateTime DateTime { get; set; }

        [Display(Name = "Customer name")]
        public string CustomerName { get; set; }
        
        [Display(Name = "Total price")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Order state")]
        public OrderState State { get; set; }
    }
}
