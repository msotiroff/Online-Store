using OnlineStore.Common.ViewModels.ShoppingCart;
using OnlineStore.Models.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Common.ViewModels.Order
{
    public class OrderCreateViewModel
    {
        [Required]
        [MinLength(ModelConstants.UserNameMinLength)]
        [Display(Name = "Full name")]
        public string CustomerName { get; set; }

        [Phone]
        [Required]
        [Display(Name = "Phone number")]
        public string CustomerPhoneNumber { get; set; }

        [Required]
        [Display(Name = "Delivery address")]
        public string DeliveryAddress { get; set; }

        public string UserId { get; set; }

        [Required]
        public string BrowserId { get; set; }

        public ShoppingCartIndexViewModel ShoppingCart { get; set; }
    }
}
