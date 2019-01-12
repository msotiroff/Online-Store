using OnlineStore.Common.ViewModels.OrderItem;
using System.Collections.Generic;

namespace OnlineStore.Common.ViewModels.ShoppingCart
{
    public class ShoppingCartIndexViewModel
    {
        public string Id { get; set; }
        
        public decimal TotalAmount { get; set; }
        
        public IEnumerable<OrderItemIndexViewModel> Items { get; set; }      
    }
}
